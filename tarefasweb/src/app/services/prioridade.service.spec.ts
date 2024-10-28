import { TestBed } from '@angular/core/testing';

import { PrioridadeService } from './prioridade.service';

describe('PrioridadeService', () => {
  let service: PrioridadeService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PrioridadeService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
