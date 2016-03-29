<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PersonalCase-GUIDandASP.AJAXandJSCode.aspx.cs" Inherits="TestProject.WebFormTest13" %>

<!DOCTYPE html>

[html] 
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"> 
<html xmlns="http://www.w3.org/1999/xhtml"> 
<head> 
<title>Js encode,decode</title> 
<meta http-equiv="Content-Type" content="text/html; charset=gb2312" /> 
</head> 
<body> 
    <form runat="server">
        <asp:ScriptManager runat="server" EnablePartialRendering="true"></asp:ScriptManager>
        <asp:Timer ID="Timer1" runat="server" Interval="1000" OnTick="Timer1_Tick"></asp:Timer> 
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                <ContentTemplate>
                    <asp:Label ID="lblTest" runat="server" Text="GUID"></asp:Label>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="Timer1" EventName="Tick" />
                </Triggers>
            </asp:UpdatePanel>
    </form>
<script> 
    a=62; 
    function encode() { 
        var code = document.getElementById('code').value; 
        code = code.replace(/[\r\n]+/g, '');
        code = code.replace(/''/g, "\\''"); 
        var tmp = code.match(/\b(\w+)\b/g); 
        tmp.sort(); 
        var dict = []; 
        var i, t = ''; 
        for(var i=0; i<tmp.length; i++) { 
            if(tmp[i] != t) dict.push(t = tmp[i]); 
        } 
        var len = dict.length; 
        var ch; 
        for(i=0; i<len; i++) { 
            ch = num(i); 
            code = code.replace(new RegExp('\\b' + dict[i] + '\\b', 'g'), ch);
            if (ch == dict[i]) dict[i] = '';
        } 
        document.getElementById('code').value = "eval(function(p,a,c,k,e,d){e=function(c){return(c<a?'':e(parseInt(c/a)))+((c=c%a)>35?String.fromCharCode(c+29):c.toString(36))};if(!''.replace(/^/,String)){while(c--)d[e(c)]=k[c]||e(c);k=[function(e){return d[e]}];e=function(){return'\\\\w+'};c=1};while(c--)if(k[c])p=p.replace(new RegExp('\\\\b'+e(c)+'\\\\b','g'),k[c]);return p}("
        + "'" + code + "'," + a + "," + len + ",'" + dict.join('|') + "'.split('|'),0,{}))";
    } 
    function num(c) { 
        return (c < a ? '' : num(parseInt(c / a))) + ((c = c % a) > 35 ? String.fromCharCode(c + 29) : c.toString(36));
    } 
    function run() { 
        eval(document.getElementById('code').value); 
    } 
    function decode() { 
        var code = document.getElementById('code').value; 
        code = code.replace(/^eval/, ''); 
        document.getElementById('code').value = eval(code);
    } 
</script> 
<textarea id="code" cols="80" rows="20"> 
</textarea><br/> 
<input type="button" onclick="encode()" value="编码"/> 
<input type="button" onclick="run()" value="执行"/> 
<input type="button" onclick="decode()" value="解码"/> 
</body> 
</html> 

