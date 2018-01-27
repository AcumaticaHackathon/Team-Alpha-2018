<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormTab.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="JM102000.aspx.cs" Inherits="Page_JM102000" Title="Untitled Page" %>

<%@ MasterType VirtualPath="~/MasterPages/FormTab.master" %>
<asp:Content ID="cont1" ContentPlaceHolderID="phDS" runat="Server">
    <px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%"
        TypeName="Jamis.Web.Face.Screens.PersonEntry"
        PrimaryView="Persons">
        <CallbackCommands>
            <px:PXDSCallbackCommand Name="Insert" PostData="Self" />
            <px:PXDSCallbackCommand CommitChanges="True" Name="Save" />
            <px:PXDSCallbackCommand Name="First" PostData="Self" StartNewGroup="True" />
            <px:PXDSCallbackCommand Name="Last" PostData="Self" />
        </CallbackCommands>
    </px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" runat="Server">
    <px:PXUploadDialog ID="pnlNewFace" Key="NewFacePanel" runat="server" Height="120px" Style="position: static" Width="560px" Caption="Face Upload" AutoSaveFile="false" RenderCheckIn="false" SessionKey="FaceFile" />
    <px:PXFormView ID="form" runat="server" DataSourceID="ds" DataMember="Persons" Width="100%" AllowAutoHide="false">
        <Template>
            <px:PXLayoutRule ID="PXLayoutRule1" runat="server" StartRow="True"></px:PXLayoutRule>
            <px:PXSelector runat="server" ID="GroupName" DataField="GroupName" />
            <px:PXSelector runat="server" ID="Name" DataField="Name" CommitChanges="true" />
            <px:PXTextEdit runat="server" ID="UserData" DataField="UserData" />
        </Template>
    </px:PXFormView>
</asp:Content>
<asp:Content ID="cont3" ContentPlaceHolderID="phG" runat="Server">
    <px:PXTab ID="tab" runat="server" Width="100%" Height="383px" DataSourceID="ds">
        <Items>
            <px:PXTabItem Text="Faces">
                <Template>
                    <px:PXImageUploader ID="edImages" runat="server" Height="300px" Width="400px" AllowUpload="false" SuppressLabel="true" />
                </Template>
            </px:PXTabItem>
        </Items>
        <AutoSize Container="Window" Enabled="True" MinHeight="150" />
    </px:PXTab>
</asp:Content>


