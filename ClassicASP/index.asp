<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <title>Number viewer</title>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.7.0/jquery.min.js"></script>
</head>
<body>
    <p>Number:</p>
    <p>
        <%
        Response.Write(Request.QueryString("phone"))
        %>
    </p>
    <script>
        $(function(){
            var token = getCookie("access_token");
            var phoneId = findGetParameter("phone");
            var settings = {
              "url": "https://localhost:7164/api/APIContacts/" + phoneId,
              "method": "GET",
              "timeout": 0,
              "headers": {
                "Authorization": "Bearer " + token
              },
            };

            $.ajax(settings).done(function (response) {
              console.log(response);
            });
        });

        function getCookie(name) {
          const value = `; ${document.cookie}`;
          const parts = value.split(`; ${name}=`);
          if (parts.length === 2) return parts.pop().split(';').shift();
        }

        function findGetParameter(parameterName) {
            var result = null,
                tmp = [];
            location.search
                .substr(1)
                .split("&")
                .forEach(function (item) {
                  tmp = item.split("=");
                  if (tmp[0] === parameterName) result = decodeURIComponent(tmp[1]);
                });
            return result;
        }
    </script>
</body>
</html>