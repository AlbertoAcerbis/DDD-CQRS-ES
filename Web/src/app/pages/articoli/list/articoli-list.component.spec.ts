import { TestBed, async } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { ArticoliListComponent } from './articoli-list.component';

describe('ArticoliListComponent', () => {
  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [ RouterTestingModule ],
      declarations: [
        ArticoliListComponent
      ]
    }).compileComponents();
  }));
  it('should create the articoliComponent', async(() => {
    const fixture = TestBed.createComponent(ArticoliListComponent);
    const app = fixture.debugElement.componentInstance;
    expect(app).toBeTruthy();
  }));
});