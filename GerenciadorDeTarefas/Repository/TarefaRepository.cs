using GerenciadorDeTarefas.Enums;
using GerenciadorDeTarefas.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GerenciadorDeTarefas.Repository
{
    public class TarefaRepository : ITarefaRepository
    {
        private readonly GerenciadorDeTarefasContext _context;

        public TarefaRepository(GerenciadorDeTarefasContext context)
        {
            _context = context;
        }

        public void AdicionarTarefa(Tarefa tarefa)
        {
            _context.Tarefa.Add(tarefa);
            _context.SaveChanges();
        }


        public Tarefa GetById(int idTarefa)
        {
            return _context.Tarefa.FirstOrDefault(t => t.Id == idTarefa);
        }

        public void RemoverTarefa(Tarefa tarefa)
        {
            _context.Tarefa.Remove(tarefa);
            _context.SaveChanges();
        }
        public void AtualizarTarefa(Tarefa tarefa)
        {
            _context.Entry(tarefa).State = EntityState.Modified;     
            _context.SaveChanges();
            _context.Entry(tarefa).State = EntityState.Detached;
        }

        public List<Tarefa> BuscarTarefas(int idUsuario, DateTime? periodoDe, DateTime? periodoAte, StatusTarefaEnum status)
        {
            return _context.Tarefa.Where(t => t.IdUsuario == idUsuario
                    && (periodoDe == null || periodoDe == DateTime.MinValue || t.DataPrevistaConclusao >= ((DateTime)periodoDe).Date)
                    && (periodoAte == null || periodoAte == DateTime.MinValue || t.DataPrevistaConclusao <= ((DateTime)periodoAte).Date)
                    && (status == StatusTarefaEnum.Todos || (status == StatusTarefaEnum.Ativos && t.DataConclusao == null)
                        || (status == StatusTarefaEnum.Concluidos && t.DataConclusao != null))
                ).ToList();
        }
    }
}
