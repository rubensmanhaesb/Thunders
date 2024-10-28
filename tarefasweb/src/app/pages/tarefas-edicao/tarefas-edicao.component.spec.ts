import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TarefasEdicaoComponent } from './tarefas-edicao.component';

describe('TarefasEdicaoComponent', () => {
  let component: TarefasEdicaoComponent;
  let fixture: ComponentFixture<TarefasEdicaoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TarefasEdicaoComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(TarefasEdicaoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
