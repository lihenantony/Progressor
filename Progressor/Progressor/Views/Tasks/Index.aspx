<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Progressor.Views.Tasks.Index" %>

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

    protected string getProgressStr(Progressor.Models.Task t)
    {
        if (t.progressMax.HasValue && t.progressIndex.HasValue)
        {
            string str = "" + ((Decimal)(1.0 * t.progressIndex / t.progressMax * 100)).ToString("#.##") + "%";
            return str;
        }
        else
        {
            return "-";
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
                    <td><% =getProgressStr(item) %></td>
                    <td><asp:UpdateProgress </td>
                    <td><% =getDueStr(item)%></td>
                    <td><% =item.getPriorityIndex()%></td>

                    <td>
                        <%-- @Html.ActionLink("Update Progress", "UpdateProgress", new { id = item.ID}) |
            @Html.ActionLink("Edit", "Edit", new { id=item.ID }) |
            @Html.ActionLink("Details", "Details", new { id=item.ID }) |
            @Html.ActionLink("Delete", "Delete", new { id=item.ID })--%>
                    </td>
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