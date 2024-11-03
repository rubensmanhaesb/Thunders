using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TarefasApp.Application.Dtos;

namespace TarefasApp.Application.Commands
{

    public class TarefaCreateCommand : IRequest<TarefaDto>
    {
        [MinLength(2, ErrorMessage = "Informe o nome com no mínimo {1} caracteres.")]
        [MaxLength(100, ErrorMessage = "Informe o nome com no máximo {1} caracteres.")]
        [Required(ErrorMessage = "Informe o nome da tarefa.")]
        public string? Nome { get; set; }

        [RegularExpression(@"^\d{4}-\d{2}-\d{2}$", 
            ErrorMessage = "Data da tarefa inválida, use o formato yyyy-MM-dd")]
        [Required(ErrorMessage = "Informe a data da tarefa.")]
        public string? Data { get; set; }

        [RegularExpression(@"^\d{2}:\d{2}$",
            ErrorMessage = "Hora da tarefa inválida, use o formato HH:mm")]
        [Required(ErrorMessage = "Informe a hora da tarefa.")]
        public string? Hora { get; set; }

        [MinLength(2, ErrorMessage = "Informe a descrição com no mínimo {1} caracteres.")]
        [MaxLength(250, ErrorMessage = "Informe a descrição com no máximo {1} caracteres.")]
        [Required(ErrorMessage = "Informe a descrição da tarefa.")]
        public string? Descricao { get; set; }

        [Range(1, 3, ErrorMessage = "A prioridade deve estar entre 1 e 3 (BAIXA=1, MEDIA=2, ALTA=3)")]
        [Required(ErrorMessage = "Informe a prioridade da tarefa.")]
        public int? Prioridade { get; set; }
    }
}
