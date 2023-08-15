<%
Dim contactsUrl
contactsUrl = "/asp/Contacts.asp"
Dim signedIn
signedIn = false
Dim identityInfoBase64, userInfo, userName, userId

identityInfoBase64=Request.Cookies("identityInfo")
identityInfoBase64=Replace(identityInfoBase64, "%3D", "=")
identityInfoBase64=Replace(identityInfoBase64, "%3D", "=")
If identityInfoBase64="" Then
    Response.Write("<script language=""javascript"">alert('Forbidden!');</script>")
Else
	userInfo = Base64Decode(identityInfoBase64)
	myarray=split(userInfo, ":",-1,1)
    userId = myarray(0)
    userName = myarray(1)

    signedIn = true
End If

If Request.QueryString("action")="logout" Then
    Response.Cookies("identityInfo").Path = "/"
    Response.Cookies("accessToken").Path = "/"
'    Response.Cookies("identityInfo")=""
'    Response.Cookies("accessToken")=""
    Response.Cookies("identityInfo").Expires = DateAdd("d",-1,now())
    Response.Cookies("accessToken").Expires = DateAdd("d",-1,now())
    Response.Redirect contactsUrl
End If
%>

<!DOCTYPE html>
<html>
<head>
	<meta charset="utf-8" />
	<title>Contact list</title>
</head>
<body>
<% If signedIn = true Then %>
    <%
    Dim objConn
    Set objConn = Server.CreateObject("ADODB.Connection")
    objConn.ConnectionString = "Provider=SQLOLEDB; Data Source=tcp:dima-contact-listdb-2023.database.windows.net,1433; Database=dima-contact-list; User ID=dima-contact-listdb; Password=Americ@n0"
    objConn.Open
    %>
        <form>
            <h1 style="color:blue">
                Welcome <%Response.Write(userName)%>
                <input type="hidden" name="action" value="logout" />
                <input type="submit" value="Sign out" />
            </h1>
        </form>
        <p>
            <%
            Dim contactsSQL, contactsObjRec
            contactsSQL = "SELECT * FROM Contacts WHERE OwnerId=" & userId & ";"
            Set contactsObjRec = objConn.Execute(contactsSQL)
            While Not contactsObjRec.EOF
              Response.Write("<div>")
              Response.Write("<h2>")
              Response.Write(contactsObjRec("Name"))
              Response.Write("</h2>")
              Response.Write("<h3>")
              Response.Write(contactsObjRec("Email"))
              Response.Write("</h3>")
              Dim phonesSQL, phonesObjRec, contactId
              Set contactId = contactsObjRec("Id")
              phonesSQL = "SELECT Id, Number, Type FROM Phones WHERE ContactId=" & contactId & ";"
              Set phonesObjRec = objConn.Execute(phonesSQL)
              While Not phonesObjRec.EOF
                  Response.Write("<p>")
                  Response.Write(phonesObjRec("Number"))
                  Response.Write("</p>")
                  phonesObjRec.MoveNext
              Wend
              phonesObjRec.Close
              Set phonesObjRec = Nothing

              Response.Write("</div>")
              contactsObjRec.MoveNext
            Wend
            contactsObjRec.Close
            Set contactsObjRec = Nothing
            %>
        </p>
    <%
    objConn.Close
    %>
<% Else %>
    <h1 style="color:#f00">Forbidden!</h1>
<% End If %>
</body>
</html>

<%
Function Base64Decode(ByVal base64String)
  'rfc1521
  '1999 Antonin Foller, Motobit Software, http://Motobit.cz
  Const Base64 = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/"
  Dim dataLength, sOut, groupBegin
  
  'remove white spaces, If any
  base64String = Replace(base64String, vbCrLf, "")
  base64String = Replace(base64String, vbTab, "")
  base64String = Replace(base64String, " ", "")
  
  'The source must consists from groups with Len of 4 chars
  dataLength = Len(base64String)
  If dataLength Mod 4 <> 0 Then
    Err.Raise 1, "Base64Decode", "Bad Base64 string."
    Exit Function
  End If

  
  ' Now decode each group:
  For groupBegin = 1 To dataLength Step 4
    Dim numDataBytes, CharCounter, thisChar, thisData, nGroup, pOut
    ' Each data group encodes up To 3 actual bytes.
    numDataBytes = 3
    nGroup = 0

    For CharCounter = 0 To 3
      ' Convert each character into 6 bits of data, And add it To
      ' an integer For temporary storage.  If a character is a '=', there
      ' is one fewer data byte.  (There can only be a maximum of 2 '=' In
      ' the whole string.)

      thisChar = Mid(base64String, groupBegin + CharCounter, 1)

      If thisChar = "=" Then
        numDataBytes = numDataBytes - 1
        thisData = 0
      Else
        thisData = InStr(1, Base64, thisChar, vbBinaryCompare) - 1
      End If
      If thisData = -1 Then
        Err.Raise 2, "Base64Decode", "Bad character In Base64 string."
        Exit Function
      End If

      nGroup = 64 * nGroup + thisData
    Next
    
    'Hex splits the long To 6 groups with 4 bits
    nGroup = Hex(nGroup)
    
    'Add leading zeros
    nGroup = String(6 - Len(nGroup), "0") & nGroup
    
    'Convert the 3 byte hex integer (6 chars) To 3 characters
    pOut = Chr(CByte("&H" & Mid(nGroup, 1, 2))) + _
      Chr(CByte("&H" & Mid(nGroup, 3, 2))) + _
      Chr(CByte("&H" & Mid(nGroup, 5, 2)))
    
    'add numDataBytes characters To out string
    sOut = sOut & Left(pOut, numDataBytes)
  Next

  Base64Decode = sOut
End Function
%>