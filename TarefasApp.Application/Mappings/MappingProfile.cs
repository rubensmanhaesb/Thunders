using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TarefasApp.Application.Commands;
using TarefasApp.Application.Dtos;
using TarefasApp.Domain.Entities;
using TarefasApp.Infra.Storage.Collections;

namespace TarefasApp.Application.Mappings
{

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            CreateMap<TarefaCreateCommand, Tarefa>()
                .AfterMap((src, dest) => {
                    dest.Id = Guid.NewGuid();
                    dest.DataHora = DateTime.Parse($"{src.Data} {src.Hora}");
                });


            CreateMap<TarefaUpdateCommand, Tarefa>()
                .AfterMap((src, dest) => {
                    dest.DataHora = DateTime.Parse($"{src.Data} {src.Hora}");
                });


            CreateMap<Tarefa, TarefaCollection>()
                .AfterMap((src, dest) => {
                    dest.DataHoraCadastro = DateTime.Now;
                });


            CreateMap<Tarefa, TarefaDto>()
                .AfterMap((src, dest) => {
                    dest.Prioridade = (Prioridade) src.Prioridade;
                });


            CreateMap<Tarefa, TarefaCollection>()
                .AfterMap((src, dest) =>
                {
                    dest.DataHoraCadastro = DateTime.Now;
                });


            CreateMap<TarefaCollection, TarefaDto>()
                .AfterMap((src, dest) =>
                {
                    dest.Prioridade = (Prioridade)src.Prioridade;
                })
                .ReverseMap();
        }
    }
}
