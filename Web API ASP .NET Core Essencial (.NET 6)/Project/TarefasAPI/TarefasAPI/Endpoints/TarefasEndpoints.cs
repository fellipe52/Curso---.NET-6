using Dapper.Contrib.Extensions;
using TarefasAPI.Data;
using static TarefasAPI.Data.TarefaContext;

namespace TarefasAPI.Endpoints
{
    public static class TarefasEndpoint
    {
        public static void MapTarefasEndpoint(this WebApplication app)
        {
            app.MapGet("/", () => $"Bem-Vindo a API Tarefas - {DateTime.Now}");

            app.MapGet("/tarefas", async (GetConnection connection) =>
            {

                var tarefas = new List<Tarefa>();
                try
                {
                    using var con = await connection();
                    tarefas = con.GetAll<Tarefa>().ToList();

                    if (tarefas is null)
                        return Results.NotFound();


                }
                catch (Exception ex)
                {

                }
                return Results.Ok(tarefas);
            });

            app.MapGet("/tarefas/{id}", async (GetConnection connection, int id) =>
            {

                using var con = await connection();
                var tarefa = con.Get<Tarefa>(id);

                if (tarefa is null)
                    return Results.NotFound();

                return Results.Ok(tarefa);
            });

            app.MapPost("/tarefas", async (GetConnection connectionGetter, Tarefa tarefa) =>
            {
                using var con = await connectionGetter();
                var id = con.Insert(tarefa);
                return id != 0 ? Results.Ok(id) : Results.NotFound();
            });

            app.MapPut("/tarefas", async (GetConnection connectionGetter, Tarefa tarefa) =>
            {
                using var con = await connectionGetter();
                var id = con.Update(tarefa);
                return Results.Ok(id);
            });

            app.MapDelete("/tarefas{id}", async (GetConnection connectionGetter, int id) =>
            {

                using var con = await connectionGetter();
                var deleted = con.Get<Tarefa>(id);

                if (deleted is null)
                    return Results.NotFound($"A tarefa não foi encontrada, data possui: {deleted} elementos");

                con.Delete(deleted);

                return Results.Ok(deleted);
            });
        }
    }
}
