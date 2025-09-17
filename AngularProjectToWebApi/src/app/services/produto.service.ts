import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Produto } from '../models/produto';

@Injectable({ providedIn: 'root' })
export class ProdutoService {
  private api = 'https://localhost:7144/api/Produto'; // ajuste a porta se necessário

  constructor(private http: HttpClient) {}

  getAll(): Observable<Produto[]> {
    return this.http.get<Produto[]>(this.api);
  }

  getById(id: number): Observable<Produto> {
    return this.http.get<Produto>(`${this.api}/${id}`);
  }

  create(p: Produto): Observable<Produto> {
    return this.http.post<Produto>(this.api, p);
  }

  update(id: number, p: Produto) {
    return this.http.put(`${this.api}/${id}`, p);
  }

  delete(id: number) {
    return this.http.delete(`${this.api}/${id}`);
  }
}
