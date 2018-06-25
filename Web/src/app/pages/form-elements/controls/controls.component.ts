import { Component, ViewEncapsulation, ViewChild } from '@angular/core';
import { IMultiSelectOption, IMultiSelectSettings, IMultiSelectTexts } from 'angular-2-dropdown-multiselect';
import { NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { NgbTimeStruct } from '@ng-bootstrap/ng-bootstrap';
import { FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'app-controls',
  templateUrl: './controls.component.html',
  styleUrls: ['./controls.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ControlsComponent {

    public firstControlModel: number[];
    public firstControlOptions: IMultiSelectOption[] = [
        { id: 1, name: 'Option 1' },
        { id: 2, name: 'Option 2' },
        { id: 3, name: 'Option 3' }
    ];

    public secondControlModel: number[];
    public secondControlSettings: IMultiSelectSettings = {
        checkedStyle: 'fontawesome',
        buttonClasses: 'btn btn-secondary btn-block',
        dynamicTitleMaxItems: 3,
        displayAllSelectedText: true,
        showCheckAll: true,
        showUncheckAll: true
    };
    public secondControlTexts: IMultiSelectTexts = {
        checkAll: 'Select all',
        uncheckAll: 'Unselect all',
        checked: 'item selected',
        checkedPlural: 'items selected',
        searchPlaceholder: 'Find',
        defaultTitle: 'Select countries',
        allSelected: 'All selected',
    };
    public secondControlOptions: IMultiSelectOption[] = [
        { id: 1, name: 'Sweden'},
        { id: 2, name: 'Norway' },
        { id: 3, name: 'Canada' },
        { id: 4, name: 'USA' }
    ];


    public thirdControlModel: number[];
    public thirdControlSettings: IMultiSelectSettings = {
        enableSearch: true,
        checkedStyle: 'checkboxes',
        buttonClasses: 'btn btn-secondary btn-block',
        dynamicTitleMaxItems: 3,
        displayAllSelectedText: true
    };
    public thirdControlTexts: IMultiSelectTexts = {
        checkAll: 'Select all',
        uncheckAll: 'Unselect all',
        checked: 'item selected',
        checkedPlural: 'items selected',
        searchPlaceholder: 'Find...',
        defaultTitle: 'Select countries using search filter',
        allSelected: 'All selected',
    };
    public thirdControlOptions: IMultiSelectOption[] = [
        { id: 1, name: 'Sweden'},
        { id: 2, name: 'Norway' },
        { id: 3, name: 'Canada' },
        { id: 4, name: 'USA' }
    ];
    public onChange() {
        console.log(this.firstControlModel);
    }


    // Basic datepicker
    // tslint:disable-next-line:member-ordering
    public model: NgbDateStruct;
    // tslint:disable-next-line:member-ordering
    public date: {year: number, month: number};
    public selectToday() {
      this.model = {year: new Date().getFullYear(), month: new Date().getMonth() + 1, day: new Date().getDate()};
    }

    // Multiple months
    // tslint:disable-next-line:member-ordering
    public displayMonths = 2;
    // tslint:disable-next-line:member-ordering
    public navigation = 'select';

    // Datepicker in a popup
    // tslint:disable-next-line:member-ordering
    public modelPopup: NgbDateStruct = {year: new Date().getFullYear(), month: new Date().getMonth() + 1, day: new Date().getDate() - 3};

    // Custom day view
    // tslint:disable-next-line:member-ordering
    public modelCustom: NgbDateStruct;
    public isWeekend(date: NgbDateStruct) {
      const d = new Date(date.year, date.month - 1, date.day);
      return d.getDay() === 0 || d.getDay() === 6;
    }
    public isDisabled(date: NgbDateStruct, current: {month: number}) {
      return date.month !== current.month;
    }

    // Disabled datepicker
    // tslint:disable-next-line:member-ordering
    public modelDisabled: NgbDateStruct = {year: new Date().getFullYear(), month: new Date().getMonth() + 1, day: new Date().getDate()};
    // tslint:disable-next-line:member-ordering
    public disabled = true;


    // Basic timepicker
    // tslint:disable-next-line:member-ordering
    public time = { hour: 13, minute: 30, second: 20 };

    // Meridian
    // tslint:disable-next-line:member-ordering
    public timeMeridian = { hour: 15, minute: 20, second: 25 };
    // tslint:disable-next-line:member-ordering
    public meridian = true;

    // Seconds
    // tslint:disable-next-line:member-ordering
    public timeSeconds: NgbTimeStruct = { hour: 16, minute: 40, second: 30 };
    // tslint:disable-next-line:member-ordering
    public seconds = true;

    // Spinners
    // tslint:disable-next-line:member-ordering
    public timeSpinners = { hour: 13, minute: 30 };
    // tslint:disable-next-line:member-ordering
    public spinners = true;

    // Custom steps
    // tslint:disable-next-line:member-ordering
    public timeCustomSteps: NgbTimeStruct = { hour: 13, minute: 30, second: 0 };
    // tslint:disable-next-line:member-ordering
    public hourStep = 1;
    // tslint:disable-next-line:member-ordering
    public minuteStep = 15;
    // tslint:disable-next-line:member-ordering
    public secondStep = 30;

    // Custom validation
    // tslint:disable-next-line:member-ordering
    public timeValidation = { hour: 11, minute: 30 };
    // tslint:disable-next-line:member-ordering
    public ctrl = new FormControl('', (control: FormControl) => {
      const value = control.value;
      if (!value) {
        return null;
      }
      if (value.hour < 12) {
        return {tooEarly: true};
      }
      if (value.hour > 13) {
        return {tooLate: true};
      }
      return null;
    });

    // Rating
    // tslint:disable-next-line:member-ordering
    public currentRate = 8;

    // Events and readonly ratings
    // tslint:disable-next-line:member-ordering
    public selected = 0;
    // tslint:disable-next-line:member-ordering
    public hovered = 0;
    // tslint:disable-next-line:member-ordering
    public readonly = false;

    // Custom star template
    // tslint:disable-next-line:member-ordering
    public currentRateCustom = 6;

    // Custom decimal rating
    // tslint:disable-next-line:member-ordering
    public currentRateDecimal = 3.14;

    // Form integration
    // tslint:disable-next-line:member-ordering
    public ctrlFormIntegration = new FormControl(null, Validators.required);
    public toggle() {
      if (this.ctrlFormIntegration.disabled) {
        this.ctrlFormIntegration.enable();
      } else {
        this.ctrlFormIntegration.disable();
      }
    }
}
