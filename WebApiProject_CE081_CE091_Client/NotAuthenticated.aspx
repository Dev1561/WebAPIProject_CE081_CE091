<%@ Page  Title="Autheticate" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="NotAuthenticated.aspx.cs" Inherits="WCFProject_CE091_CE081_Client.NotAuthenticated" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class="jumbotron" style="background-color: darkslategrey">
        <h2> You are not logged in...</h2>
        <hr />
        <p> <a href="Account/Login.aspx" class="btn btn-primary btn-lg"> LogIn &raquo </a> </p>
    </div>
</asp:Content>