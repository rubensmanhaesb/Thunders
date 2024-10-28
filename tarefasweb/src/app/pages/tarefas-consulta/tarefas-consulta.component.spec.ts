import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TarefasConsultaComponent } from './tarefas-consulta.component';

describe('TarefasConsultaComponent', () => {
  let component: TarefasConsultaComponent;
  let fixture: ComponentFixture<TarefasConsultaComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TarefasConsultaComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(TarefasConsultaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
