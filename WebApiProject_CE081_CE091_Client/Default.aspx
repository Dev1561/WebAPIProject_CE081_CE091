<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WCFProject_CE091_CE081_Client._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron" style="background-color: darkslategrey">
        <h1>Protected Notes</h1>
        <p class="lead">Protected Notes is an application where you can upload your notes/messages in text format. Your data is uploaded safely in an encrypted form. Your data is ecrypted with very secure cryptographic methods.</p>
    </div>

    <div class="row">
        <div class="col-md-8">
            <h2>Getting started</h2>
            <p>
                -> Select your file<br />
                -> Enter 32 character long Alphanumeric key and keep it safe with you.<br />
                -> Upload you file<br />
                <br /><br />
                Hurray!!! Your data is secured with us. Come back with the 32 character long key to get your secured file.
            </p>
        </div>
        <%--
        <div class="col-md-4">
            <h2>Contact us</h2>
        </div>
        --%>
    </div>

</asp:Content>
