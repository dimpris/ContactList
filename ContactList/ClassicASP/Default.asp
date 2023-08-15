<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <title>Number viewer</title>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.7.0/jquery.min.js"></script>
</head>
<body>
    <div class="loginform" style="display: none">
        <p>Login:</p>
        <p>
            <input id="login" />
        </p>
        <p>Password:</p>
        <p><input id="password" type="password" /></p>
        <p><input id="loginBtn" type="button" value="Login" /></p>
    </div>
    <div class="output" style="display: none"></div>
    <script>
        // const host = "https://localhost:7164/"; //https://contactlist20230804150242.azurewebsites.net/
        const host = "https://contactlist20230815140706.azurewebsites.net/";
        $(function(){
            start();

            $("#loginBtn").click(function(){
                $(".loginform").hide();
                var settings = {
                  "url": host + "api/ContactList/Login",
                  "method": "POST",
                  "timeout": 0,
                  "headers": {
                    "Content-Type": "application/json"
                  },
                  "data": JSON.stringify({
                    "login": $("#login").val(),
                    "password": $("#password").val()
                  }),
                };

                $.ajax(settings).done(function (response) {
                  localStorage.setItem("token", response.accessToken);
                  start();
                });
            });
        });

        function start() {
            var token = localStorage.getItem("token");
            if (token) {
                var settings = {
                  "url": host + "api/ContactList",
                  "method": "GET",
                  "timeout": 0,
                  "headers": {
                    "Authorization": "Bearer " + token
                  },
                };

                $.ajax(settings).done(function (response) {
                  for(var r of response){
                    $(".output").append("<h2>" + r.name + "</h2>");
                    for (var p in r.phones) {
                        $(".output").append("<p>" + r.phones[p] + "</p>");
                    }
                  }
                  $(".output").show();
                });
            } else {
                $(".loginform").show();
            }
        }
    </script>
</body>
</html>