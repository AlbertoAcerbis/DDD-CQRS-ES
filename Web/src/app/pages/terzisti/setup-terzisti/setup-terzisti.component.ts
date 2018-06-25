import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { FormGroup, FormControl, FormBuilder, Validators} from '@angular/forms';
import { Location } from '@angular/common';
import { CustomValidators } from 'ng2-validation';

import { Terzista } from '../../../shared/models/terzista/terzista';
import { UsersServices } from '../../../shared/services/users/users.services';
import { FormValidationService } from '../../../shared/services/utilities/formvalidation.service';

@Component({
  selector: 'app-setup-terzisti',
  templateUrl: './setup-terzisti.component.html',
  styleUrls: ['./setup-terzisti.component.scss'],
  encapsulation: ViewEncapsulation.None
})

export class SetupTerzistiComponent implements OnInit {

  public setupTerzistaForm: FormGroup;
  public terzista = new Terzista;

  public codiceUtente = '';
  public azienda = '';
  public codiceSap = '';
  public tipologiaContratto;
  public numeroContratto = '';
  public dataContratto: Date;
  public codiceSapSc = '';
  public aziendaUnipersonale;
  public numeroDipendenti;
  public indirizzoAzienda = '';
  public localita = '';
  public indirizzoEmail = '';
  public cassaEdile;
  public cittaCassaEdile = '';
  public cittaCameraCommercio = '';
  public numeroCameraCommercio = '';
  public niCommodity: Date;
  public niCustom: Date;
  public modRCommodity: Date;
  public modRCustom: Date;
  public riparazioni: Date;
  public modT: Date;
  public area;
  public dataAttivazione: Date;
  public emailAutomatica;
  public terzistaPagante;
  public disabilitato = false;

  constructor(private _formBuilder: FormBuilder,
              private _userService: UsersServices,
              private _location: Location) { }

  ngOnInit() {
    this.setupTerzistaForm = this._formBuilder.group({
      // 'username': ['', Validators.required],
      // 'fullname': ['', Validators.required],
      // 'password': password,
      // 'confirmPassword': confirmPassword,
      // 'email': ['', Validators.compose([Validators.required, CustomValidators.email])],
    });
  }

}
