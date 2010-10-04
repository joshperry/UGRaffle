using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RaffleLib.Domain;
using RaffleLib.Domain.Entities;
using RaffleLib.Domain.Queries;
using RaffleWeb.Controllers;
using RaffleWeb.Models;
using WebLib.Auth;
using Machine.Specifications;
using It = Machine.Specifications.It;

namespace RaffleWebTests
{
    [Subject("Login Member")]
    public class when_valid_login_credentials_provided : with_member_login_mocks
    {
        Because of =
            () => new MemberController(null, AuthMock.Object).Login(GetByCredentialsQuery, LoginModel, null);

        It should_set_nonpersistent_auth_cookie = () => AuthMock.Verify(x => x.SetAuthCookie(LoginModel.Email, false));
    }

    [Subject("Login Member")]
    public class when_invalid_login_credentials_provided : with_member_login_mocks
    {
        static ViewResult result;

        Because of = () => result = new MemberController(null, AuthMock.Object).Login(GetByCredentialsQuery,
                                    new LoginViewModel
                                        {
                                            Email = LoginModel.Email,
                                            Password = "badd"
                                        }, null) as ViewResult;
        
        It should_not_set_auth_cookie = () => 
            AuthMock.Verify(x => x.SetAuthCookie(Moq.It.IsAny<string>(), Moq.It.IsAny<bool>()), Times.Never());

        It should_redirect_to_default_view = () => result.ViewName.ShouldEqual(string.Empty);
    }

    [Subject("Login Member")]
    public class when_successfully_logged_in_with_redirect_url : with_member_login_mocks
    {
        private const string REDIRECT_URL = "/blah";
        static RedirectResult result;

        Because of = ()=> result = new MemberController(null, AuthMock.Object).Login(GetByCredentialsQuery, LoginModel, REDIRECT_URL) as RedirectResult;

        It should_redirect_to_specified_url = () => result.Url.ShouldEqual(REDIRECT_URL);
    }

    [Subject("Login Member")]
    public class when_successfully_logged_in_without_redirect_url : with_member_login_mocks
    {
        static RedirectResult result;

        Because of = ()=> result = new MemberController(null, AuthMock.Object).Login(GetByCredentialsQuery, LoginModel, null) as RedirectResult;

        It should_redirect_to_root = () => result.Url.ShouldEqual("~/");
    }

    [Subject("Create Member")]
    public class when_creating_nonexistant_new_member : with_create_member_mocks
    {
        private static RedirectResult result;

        Because of = () => result =
            new MemberController(MemberRepoMock.Object, null).Create(GetByEmailQueryMock.Object, NewMember) as RedirectResult;

        It should_check_for_existing_member_with_same_email_address = () =>
            GetByEmailQueryMock.Verify(x => x.Result(NewMember.Email));

        It should_save_new_member_to_repo = () => MemberRepoMock.Verify(x => x.Save(NewMember));

        It should_redirect_to_site_root = () => result.Url.ShouldEqual("~/");
    }

    [Subject("Create Member")]
    public class when_creating_member_with_same_emails_as_existing_member : with_create_member_mocks
    {
        private static ViewResult result;

        Because of = () => result = new MemberController(MemberRepoMock.Object, null).Create(GetByEmailQueryMock.Object, ExistingMember) as ViewResult;

        It should_check_for_existing_member_with_same_email_address = () =>
            GetByEmailQueryMock.Verify(x => x.Result(ExistingMember.Email));

        It should_not_save_new_member_to_repo = () => MemberRepoMock.Verify(x => x.Save(NewMember), Times.Never());

        It should_redisplay_default_view = () => result.ViewName.ShouldEqual(string.Empty);

        It should_provide_already_entered_data = () => result.ViewData.Model.ShouldBeTheSameAs(ExistingMember);
    }

    public abstract class with_create_member_mocks
    {
        protected static readonly Member NewMember = new Member {Email = "me@example.com"};
        protected static readonly Member ExistingMember = new Member {Email = "josh@6bit.com"};
        protected static readonly Mock<IGetMemberByEmail> GetByEmailQueryMock = new Mock<IGetMemberByEmail>();
        protected static readonly Mock<IEntityRepository<Member>> MemberRepoMock = new Mock<IEntityRepository<Member>>();

        Establish context = () =>
        {
            GetByEmailQueryMock.Setup(x => x.Result(Moq.It.IsAny<string>())).Returns((Member)null);
            GetByEmailQueryMock.Setup(x => x.Result(ExistingMember.Email)).Returns(ExistingMember);
        };
    }

    public abstract class with_member_login_mocks
    {
        protected static readonly LoginViewModel LoginModel = new LoginViewModel
                                                                  {Email = "me@example.com", Password = "password"};
        protected static readonly Mock<IFormsAuthentication> AuthMock = new Mock<IFormsAuthentication>();
        protected static IGetMemberByCredentials GetByCredentialsQuery;

        Establish context = () =>
                                {
                                    var queryMock = new Mock<IGetMemberByCredentials>();
                                    queryMock.Setup(x => x.Result(LoginModel.Email, Moq.It.IsAny<string>())).Returns((Member)null);
                                    queryMock.Setup(x => x.Result(LoginModel.Email, LoginModel.Password)).Returns(new Member());
                                    GetByCredentialsQuery = queryMock.Object;
                                };
    }
}
