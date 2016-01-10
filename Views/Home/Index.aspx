<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<UptimeTest.Models.SearchModel>" %>

<asp:Content ID="title" ContentPlaceHolderID="TitleContent" runat="server">
    Index
</asp:Content>

<asp:Content ID="body" ContentPlaceHolderID="MainContent" runat="server">

<% using (Html.BeginForm()) { %>
    <h3 class="text-muted">Otsi Amazon'i toodete hulgast...</h3>
    <div class="row">
        <div class="col-lg-7">
            <div class="input-group">
                <%: Html.TextBox("search", Model.SearchKeyword, new { placeholder = "Otsi...", @class = "form-control" }) %>
                <span class="input-group-btn">
                    <button type="submit" class="btn btn-default"><i class="glyphicon glyphicon-search"></i></button>
                </span>
            </div>
        </div>

    </div>
      
<% } %>
<% if (Model.AmazonItem != null && Model.AmazonItem.Count > 0)
   { %>
    <h4 class="text-muted" >Otsingu tulemused</h4> 
    <div  class="pull-right" style="margin-top: -40px;">
        <select id="exchange-rates" class="form-control" style="width:80px;"></select>
    </div>
    <div class="table-responsive search-results">
         <table class="table table-bordered table-condensed table-striped" data-active-page="1">
            <thead>
                <tr>
                    <th>#</th>
                    <th>Nimetus</th>
                    <th>Hind</th>
                </tr>
            </thead>
            <tbody>
            <%  
                foreach (UptimeTest.Amazon.AmazonItem item in Model.AmazonItem)
                {
            %>
                <tr data-page="<%=item.PageNumber%>" style="display:none;">
                    <td class="text-right"><%=item.Index%></td>
                    <td><%= item.Title%></td>
                    <td class="amount text-right" data-amount="<%= item.FormatedAmount  %>" data-currency="<%= item.Currency %>"><%= item.FormatedAmount%></td>
                </tr>
            <%      
                }
            %>
                <tr data-page="<%=Model.PagesCount%>" style="display:none;">
                   <td colspan="3">
                    Rohkem tulemusi: <a href="<%=Model.SearchMoreUrl %>"><%=Model.SearchMoreUrl %></a>
                   </td> 
                </tr>
            </tbody>
         </table>
         <ul class="pagination">
            <li>
              <a href="#" aria-label="first" data-action="first">
                <span aria-hidden="true">&laquo;</span>
              </a>
            </li>
            <% for (int i = 0; i < Model.PagesCount; i++){ %>
               <li><a href="#" data-action="<%=(i + 1) %>"><%=(i + 1) %></a></li>
            <% } %>
        </ul>
    </div>
<% } %>  
</asp:Content>
