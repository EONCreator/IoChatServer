using Microsoft.AspNetCore.Mvc;
using IoChatServer.Domain.Repositories;

namespace IoChatServer.Primitives;

public abstract class BaseController<TEntity> : Controller
    where TEntity : class
{
    public IRepository Repository;

    public BaseController(IRepository repository)
    {
        Repository = repository;
    }
}