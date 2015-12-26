<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Progressor.Views.Tasks.Index" %>

<%@ Register assembly="EO.Web" namespace="EO.Web" tagprefix="eo" %>

<!DOCTYPE html>

<%--<%@ Page Language="C#" %>--%>

<script runat="server">
    protected string getPriLvl(Progressor.Models.Task t)
    {
        string lvl = "row-lvl";
        if (t.getPriorityIndex() == 0)
        {
            lvl += "0";
        }
        else if (t.getPriorityIndex() < 100)
        {
            lvl += "1";
        }
        else if (t.getPriorityIndex() < 250)
        {
            lvl += "2";
        }
        else if (t.getPriorityIndex() < 500)
        {
            lvl += "3";
        }
        else
        {
            lvl += "4";
        }
        return lvl;
    }

    protected int getProgress(Progressor.Models.Task t)
    {
        if (t.progressMax.HasValue && t.progressIndex.HasValue)
        {
            return ((int)(1.0 * t.progressIndex / t.progressMax * 100));
        }
        else
        {
            return 0;
        }
    }

    protected string getDueStr(Progressor.Models.Task t)
    {
        if (t.dueDate.HasValue)
        {
            string str = "" + (int)(t.dueDate.Value - DateTime.Now).TotalDays;
            return str;
        }
        else
        {
            return "-";
        }
    }

    
</script>



<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table class="table">
                <tr>
                    <th>Name</th>
                    <th>Status</th>
                    <th>Progress</th>
                    <th>Due</th>
                    <th>Priority</th>
                </tr>
                <%foreach (var item in this.ViewData.Model)
                    { %>
                <tr class="<% =getPriLvl(item).ToString() %>">
                    <td><% =item.name.ToString() %></td>
                    <td><% =item.taskStatus.ToString() %></td>
                    <%--<td><% =getProgressStr(item) %></td>--%>
                    <td><eo:ProgressBar runat="server" Margin="0,0,0,0" Height="25px" Width="100px" HorizontalAlignment="Left" Value= '<%# getPriLvl(item)%>' ShowContent="True" ControlSkinID="Style1" /></td>
                    <td><% =getDueStr(item)%></td>
                    <td><% =item.getPriorityIndex()%></td>

                </tr>

                <% } %>
            </table>
            


        </div>
    </form>
</body>
</html>


<style>
    tr.row-lvl0 {
        background-color: lightgray;
    }

    tr.row-lvl1 {
        background-color: lightskyblue;
    }

    tr.row-lvl2 {
        background-color: greenyellow;
    }

    tr.row-lvl3 {
        background-color: yellow;
    }

    tr.row-lvl4 {
        background-color: orangered;
    }
</style>