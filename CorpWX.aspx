<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CorpWX.aspx.cs" Inherits="WebWX.CorpWX" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>测试页面</title>
    <link rel="shortcut icon" href="#" />
    <script src="jquery-3.3.1.min.js"></script>

</head>
<body>

    <script type="text/javascript">

        $(function () {
            var code = "";
            var access_token = "<%= AccessToken%>";


            //debugger;
            code = GetQueryString("code");//回调网址自动获取code信息
            //alert(code);


            //alert(access_token);

            var userid = "<%= Userid%>";
            //userid = "213";
            alert(userid);
            <%--alert("flag =" + "<%= flag%>");--%>

        });

        function GetQueryString(name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
            var r = window.location.search.substr(1).match(reg);
            if (r != null) return decodeURI(r[2]); return null;
        }

    </script>


    <form id="form1" runat="server">
        <div>
            测试页面
        </div>
    </form>


</body>
</html>
