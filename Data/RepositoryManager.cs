﻿using Contracts.Repositories;
using Data.Repositories;

namespace Data;

public class RepositoryManager : IRepositoryManager
{
    private readonly Lazy<IGroupRepository> _groupRepository;
    private readonly Lazy<IMenuRepository> _menuRepository;
    private readonly Lazy<IOrderRepository> _orderRepository;
    private readonly RepositoryContext _repositoryContext;
    private readonly Lazy<ISecurityRepository> _securityRepository;
    private readonly Lazy<IUserRepository> _userRepository;

    public IUserRepository User => _userRepository.Value;
    public IMenuRepository Menu => _menuRepository.Value;
    public IOrderRepository Order => _orderRepository.Value;
    public ISecurityRepository Security => _securityRepository.Value;
    public IGroupRepository Groups => _groupRepository.Value;

    public async Task SaveAsync()
    {
        await _repositoryContext.SaveChangesAsync();
    }

    public RepositoryManager(RepositoryContext repositoryContext)
    {
        _repositoryContext = repositoryContext;
        _userRepository = new Lazy<IUserRepository>(() => new UserRepository(repositoryContext));
        _menuRepository = new Lazy<IMenuRepository>(() => new MenuRepository(repositoryContext));
        _orderRepository = new Lazy<IOrderRepository>(() => new OrderRepository(repositoryContext));
        _securityRepository = new Lazy<ISecurityRepository>(() => new SecurityRepository(repositoryContext));
        _groupRepository = new Lazy<IGroupRepository>(() => new GroupRepository(repositoryContext));
    }
}