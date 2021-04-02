<%@ Page Title="Encryption" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Encryption.aspx.cs" Inherits="WCFProject_CE091_CE081_Client.WebForm1" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>. </h2>

    <div class="row">
        <div class="col-md-8">
            <section id="loginForm">
                <div class="form-horizontal">
                    <h4>Enter your text here to encrypt it. </h4>
                    <hr />
                    <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="false">
                        <p class="text-danger">
                            <asp:Literal runat="server" ID="FailureText" />
                        </p>
                    </asp:PlaceHolder>
                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="TextBox1" CssClass="col-md-2 control-label"> Key : </asp:Label>
                        <div class="col-md-10">
                            <asp:TextBox ID="TextBox1" runat="server" CssClass="form-control" placeholder="Key of length 32 without any spaces" Width="100%" TextMode="Password"></asp:TextBox>
                            
                            <asp:Button runat="server" ID="GenKey" OnClick="GenKey_Click" Text="Generate Key"/>
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="FileUpload1" CssClass="col-md-2 control-label"> File : </asp:Label>
                        <div class="col-md-10">
                            <asp:FileUpload ID="FileUpload1" runat="server" CssClass="form-control" />
                            
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-offset-2 col-md-8">
                            <asp:Button runat="server" OnClick="EncryptData" Text="Encrypt Data" CssClass="btn btn-primary" />
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" ID="Label1" > </asp:Label>
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" ID="Label2" > </asp:Label>
                    </div>
                </div>
            </section>
            <asp:Label runat="server" ID="Label3">  </asp:Label>
        </div>
    </div>
</asp:Content>
