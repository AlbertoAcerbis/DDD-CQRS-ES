using System;
using System.Collections.Generic;
using System.Linq;
using Paramore.Brighter;
using Xunit;

namespace FourSolid.Cqrs.Anagrafiche.Domain.Test
{
    /// <summary>
    /// https://github.com/luizdamim/NEventStoreExample/tree/master/NEventStoreExample.Test
    /// </summary>
    /// <typeparam name="TCommand"></typeparam>
    public abstract class EventSpecification<TCommand> where TCommand : Command
    {
        protected Exception Caught { get; private set; }

        protected InMemoryEventRepository Repository { get; private set; }

        protected bool RunTest(InMemoryEventRepository inMemoryRepository)
        {
            this.Caught = null;
            this.Repository = inMemoryRepository;
            this.Repository.SetGivenEvents(this.Given().ToList());
            var handler = this.OnHandler();

            try
            {
                handler.HandleAsync(this.When()).Wait();
                var expected = this.Expect().ToList();
                var published = this.Repository.Events;
                CompareEvents(expected, published);
            }
            catch (Exception exception)
            {
                this.Caught = exception;
                return false;
            }

            return true;
        }

        protected abstract IEnumerable<Event> Given();

        protected abstract TCommand When();

        protected abstract RequestHandlerAsync<TCommand> OnHandler();

        protected abstract IEnumerable<Event> Expect();

        private static void CompareEvents(ICollection<Event> expected, ICollection<Event> published)
        {
            Assert.Equal(published.Count, expected.Count);
            Assert.True(published.Count == expected.Count, "Different number of expected/published events.");

            //var compareObjects = new CompareLogic();

            var eventPairs = expected.Zip(published, (e, p) => new { Expected = e, Produced = p });
            foreach (var events in eventPairs)
            {
                if (events.Expected.GetType() != events.Produced.GetType())
                    Assert.True(false, $"Event Expected {events.Expected.GetType()} - Event Produced {events.Produced.GetType()}");

                var chkEvents =
                    PublicInstancePropertiesEqual(events.Expected, events.Produced, "Id");

                //var result = compareObjects.Compare(events.Expected, events.Produced);
                if (!chkEvents)
                {
                    //Assert.True(false, result.DifferencesString);
                    Assert.True(false, "Different Properties");
                }
            }
        }

        public static bool PublicInstancePropertiesEqual<T>(T self, T to, params string[] ignore) where T : class
        {
            if (self != null && to != null)
            {
                var type = typeof(T);
                var ignoreList = new List<string>(ignore);
                return !(from pi in
                        type.GetProperties(System.Reflection.BindingFlags.Public |
                                           System.Reflection.BindingFlags.Instance)
                    where !ignoreList.Contains(pi.Name)
                    let selfValue = type.GetProperty(pi.Name).GetValue(self, null)
                    let toValue = type.GetProperty(pi.Name).GetValue(to, null)
                    where selfValue != toValue && (selfValue == null || !selfValue.Equals(toValue))
                    select selfValue).Any();
            }
            return self == to;
        }

        // https://stackoverflow.com/questions/506096/comparing-object-properties-in-c-sharp
        //public static bool PublicInstancePropertiesEqual<T>(T self, T to, params string[] ignore) where T : class
        //{
        //    if (self != null && to != null)
        //    {
        //        Type type = typeof(T);
        //        List<string> ignoreList = new List<string>(ignore);
        //        foreach (System.Reflection.PropertyInfo pi in type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
        //        {
        //            if (!ignoreList.Contains(pi.Name))
        //            {
        //                object selfValue = type.GetProperty(pi.Name).GetValue(self, null);
        //                object toValue = type.GetProperty(pi.Name).GetValue(to, null);

        //                if (selfValue != toValue && (selfValue == null || !selfValue.Equals(toValue)))
        //                {
        //                    return false;
        //                }
        //            }
        //        }
        //        return true;
        //    }
        //    return self == to;
        //}
    }
}