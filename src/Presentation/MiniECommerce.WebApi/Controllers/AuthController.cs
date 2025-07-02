//using microsoft.aspnetcore.mvc;
//using miniecommerce.application.dtos.appuserdto;
//using miniecommerce.application.services.interfaces;
//using miniecommerce.domain.dtos;

//namespace miniecommerce.webapi.controllers
//{
//    [apicontroller]
//    [route("api/auth")]
//    public class authcontroller : controllerbase
//    {
//        private readonly iauthservice _authservice;

//        public authcontroller(iauthservice authservice)
//        {
//            _authservice = authservice;
//        }

//        [httppost("register")]
//        public async task<iactionresult> register(registerdto dto)
//        {
//            var result = await _authservice.registerasync(dto);
//            if (!result.issuccess)
//                return badrequest(result.message);

//            return ok(result.data);
//        }

//        [httppost("login")]
//        public async task<iactionresult> login(logindto dto)
//        {
//            var result = await _authservice.loginasync(dto);
//            if (!result.issuccess)
//                return unauthorized(result.message);

//            return ok(result.data);
//        }

//        [httpget("me")]
//        public async task<iactionresult> getprofile()
//        {
//            var userid = user.getuserid(); // extension method needed to get user id from claims
//            if (userid == null)
//                return unauthorized();

//            var result = await _authservice.getprofileasync(userid.value);
//            if (!result.issuccess)
//                return notfound(result.message);

//            return ok(result.data);
//        }
//    }
//}
