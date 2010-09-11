using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using RaffleLib.Domain.Entities;
using RaffleLib.Domain;
using RaffleLib.Domain.Queries;
using RaffleLib;
using System.Configuration.Provider;
using System.Transactions;
using Ninject;

namespace RaffleWeb.Infrastructure.Auth
{
    public class MemberRoleProvider : RoleProvider
    {
        IEntityRepository<Member> _repo;
        public MemberRoleProvider(IEntityRepository<Member> repo)
        {
            _repo = repo;
        }

        public MemberRoleProvider()
        {

        }

        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {

            base.Initialize(name, config);
        }

        IEntityRepository<Member> Repo
        {
            get
            {
                return _repo ?? MvcApplication.Kernel.Get<IEntityRepository<Member>>();
            }
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            var members = Repo.Query.Where(m => usernames.Contains(m.Email));

            if (members.Count() != usernames.Length)
                throw new ProviderException("Unknown username(s) provided");

            using (var tx = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                foreach (var member in members)
                    foreach (var role in roleNames)
                    {
                        if (role == null)
                            throw new ArgumentNullException("roleNames", "Cannot have a null role name");
                        else if(role == string.Empty)
                            throw new ArgumentException("Cannot have an empty role name", "roleNames");

                        if (!member.Roles.Contains(role))
                        {
                            member.Roles.Add(role);
                            Repo.Save(member);
                        }
                    }

                tx.Complete();
            }
        }

        public override string ApplicationName { get; set; }

        public override void CreateRole(string roleName)
        {
            return;
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            Ensure.Argument(() => roleName).IsNotNull().IsNotEmpty();

            var members = new GetMembersInRole(Repo).Result(roleName);

            if (throwOnPopulatedRole)
            {
                if (members.Count() > 0)
                    throw new ProviderException("There are still members in this role");
            }
            else
            {
                foreach (var member in members)
                {
                    member.Roles.Remove(roleName);
                    Repo.Save(member);
                }
            }

            return true;
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            return Repo.Query.Where(m => m.Roles.Contains(roleName)).Select(m => m.Email).ToArray();
        }

        public override string[] GetAllRoles()
        {
            return Repo.Query.SelectMany(m => m.Roles).Distinct().ToArray();
        }

        public override string[] GetRolesForUser(string username)
        {
            Ensure.Argument(() => username).IsNotNull().IsNotEmpty();

            var member = new GetMemberByEmail(Repo).Result(username);
            if (member == null)
                throw new ProviderException("A user with that email does not exist");

            return member.Roles.ToArray();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            return new GetMembersInRole(Repo)
                .Result(roleName)
                .Select(m => m.Email)
                .ToArray();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            Ensure.Argument(() => username).IsNotNull().IsNotEmpty();
            Ensure.Argument(() => roleName).IsNotNull().IsNotEmpty();

            var member = new GetMemberByEmail(Repo).Result(username);
            if (member == null)
                throw new ProviderException("A user with that email does not exist");

            return member.Roles.Contains(roleName);
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            var members = Repo.Query.Where(m => usernames.Contains(m.Email));

            if (members.Count() != usernames.Length)
                throw new ProviderException("Unknown username(s) provided");

            using (var tx = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                foreach (var member in members)
                    foreach (var role in roleNames)
                    {
                        if (role == null)
                            throw new ArgumentNullException("roleNames", "Cannot have a null role name");
                        else if (role == string.Empty)
                            throw new ArgumentException("Cannot have an empty role name", "roleNames");

                        if (member.Roles.Contains(role))
                        {
                            member.Roles.Remove(role);
                            Repo.Save(member);
                        }
                    }

                tx.Complete();
            }
        }

        public override bool RoleExists(string roleName)
        {
            return true;
        }
    }
}