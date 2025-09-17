import { Component, OnInit } from '@angular/core';
import { ProdutoService } from './services/produto.service';
import { Produto } from './models/produto';
import { ConfirmationService, MessageService } from 'primeng/api';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  standalone: false,
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  title = 'AngularProjectToWebApi';
  produtos: Produto[] = [];
  loading = false;
  error = '';

  // Para formulário de cadastro/edição
  produtoForm: Produto = { nome: '' };
  editando: boolean = false;

  constructor(private produtoService: ProdutoService, private confirmationService: ConfirmationService,
    private messageService: MessageService) { }

  ngOnInit(): void {
    this.fetchProdutos();
  }

  fetchProdutos() {
    this.loading = true;
    this.produtoService.getAll().subscribe({
      next: (data) => {
        this.produtos = data;
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Erro ao carregar produtos';
        this.loading = false;
      }
    });
  }

  salvarProduto() {
    if (this.editando && this.produtoForm.id) {
      this.produtoService.update(this.produtoForm.id, this.produtoForm).subscribe({
        next: () => {
          this.fetchProdutos();
          this.cancelarEdicao();
          this.messageService.add({severity:'success', summary:'Atualizado', detail:'Produto atualizado com sucesso'});
        },
        error: () => this.error = 'Erro ao atualizar produto'
      });
    } else {
      this.produtoService.create(this.produtoForm).subscribe({
        next: () => {
          this.fetchProdutos();
          this.limparFormulario();
          this.messageService.add({severity:'success', summary:'Cadastrado', detail:'Produto cadastrado com sucesso'});
        },
        error: () => this.error = 'Erro ao cadastrar produto'
      });
    }
  }

  editarProduto(p: Produto) {
    this.produtoForm = { ...p };
    this.editando = true;
  }

  cancelarEdicao() {
    this.limparFormulario();
    this.editando = false;
  }

  limparFormulario() {
    this.produtoForm = { nome: '', preco: 0, quantidade: 0 };
  }

  removerProduto(id: number | undefined) {
    if (!id) return;
    this.confirmationService.confirm({
      header: 'Danger Zone',
      message: 'Deseja remover este produto?',
      icon: 'pi pi-exclamation-triangle',
      rejectButtonProps: {
        label: 'Cancel',
        severity: 'secondary',
        outlined: true,
      },
      acceptButtonProps: {
        label: 'Delete',
        severity: 'danger',
      },
      accept: () => {
        this.produtoService.delete(id).subscribe({
          next: () => {
            this.fetchProdutos();
            this.messageService.add({severity:'success', summary:'Removido', detail:'Produto removido com sucesso'});
          },
          error: () => this.error = 'Erro ao remover produto'
        });
      }
    });
  }
}
