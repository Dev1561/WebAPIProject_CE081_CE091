<%@ Page Language="C#" Title="Decryption" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EmptyView.aspx.cs" Inherits="WCFProject_CE091_CE081_Client.EmptyView" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>. </h2>
    <hr />

    <div class="jumbotron" style="background-color: darkslategrey">
        <h2> You do not have any files....</h2>
        <hr />
        <p> Click <a href="Encryption.aspx" class="btn btn-primary btn-lg"> here &raquo </a> to add your files. </p>
    </div>
</asp:Content>
