<%@ Page Title="Decryption" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Decryption.aspx.cs" Inherits="WCFProject_CE091_CE081_Client.WebForm2" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>. </h2>
    <hr />
    <br />
    <div>
        <asp:GridView ID="GridView1" runat="server" HeaderStyle-Font-Size="XX-Large" CssClass="table table-light bg-light" Width="582px" OnRowCommand="GridView1_RowCommand" Style="margin-left: 102px" OnRowDeleting="OnRowDeleting" >
            <Columns>
                <asp:ButtonField Text="Decrypt File" CommandName="Decrypt" ItemStyle-Width="200px" ButtonType="Button">
                    <ControlStyle CssClass="btn btn-primary"></ControlStyle>
                    <ItemStyle Width="200px"></ItemStyle>
                </asp:ButtonField>
                <asp:CommandField ShowDeleteButton="true" ButtonType="Button"/>
            </Columns>

        </asp:GridView>
        <br />

        <div class="row">
            <div class="col-md-8">
                <section id="loginForm">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <asp:Label runat="server" ID="Label1" AssociatedControlID="TextBox1" CssClass="col-md-2 control-label"> Key : </asp:Label>
                            <div class="col-md-10">
                                <asp:TextBox ID="TextBox1" runat="server" TextMode="Password" CssClass="form-control" placeholder="Enter the key with which you decrypted the file"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </section>
            </div>
        </div>

        <asp:Panel ID="Panel1" runat="server">
            <div style="background-color: darkslategray; color: limegreen; padding: 8px;">
                <div class="row" style="margin: 10px ; ">
                    <h4 class="col-md-10 px-md-5">Decrypted Text of File
                    </h4>
                    <h4 class="col-md-2" >
                        <asp:Button Width="100%" runat="server" ID="download" Text="Download" OnClick="download_Click" />
                    </h4>
                </div>
                <hr />
                <div style="white-space: pre-line">
                    <%=dec_data %>
                </div>
            </div>
        </asp:Panel>
        <br />
    </div>
</asp:Content>
