<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormTab.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="JM101000.aspx.cs" Inherits="Page_JM101000" Title="Untitled Page" %>

<%@ MasterType VirtualPath="~/MasterPages/FormTab.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" runat="Server">
    <px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%"
        TypeName="Jamis.Web.Face.Screens.GroupEntry"
        PrimaryView="Groups">
        <CallbackCommands>
            <px:PXDSCallbackCommand Name="Identify"  Visible="false" />
        </CallbackCommands>
    </px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" runat="Server">
    <px:PXFormView ID="form" runat="server" DataSourceID="ds" DataMember="Groups" Width="100%" Height="100px" AllowAutoHide="false">
        <Template>
            <px:PXLayoutRule ID="PXLayoutRule1" runat="server" StartRow="True"></px:PXLayoutRule>
            <px:PXSelector runat="server" ID="CstPXSelector3" DataField="Name" />
            <px:PXTextEdit runat="server" ID="CstPXTextEdit4" DataField="UserData" />
            <px:PXButton runat="server" ID="btnIdentify" Text="Identify">
                <ClientEvents Click="btnIdentifyClick" />
            </px:PXButton>
        </Template>
    </px:PXFormView>
    
    <script type="text/javascript">

        function btnIdentifyClick(sender, e)
        {
            if (typeof (window.top.FaceApi) != "undefined" && typeof (window.faceApi) == "undefined")
            {
                window.faceApi = new window.top.FaceApi();
            }

            if (typeof (window.faceApi) != "undefined")
            {
                window.faceApi.open(function (api)
                {
                    if (api.data)
                    {
                        px_all["<%= ds.ClientID %>"].executeCallback('Identify', api.data);
                    }
                });
            }

            e.cancel = true;
        }

    </script>
</asp:Content>
<asp:Content ID="cont3" ContentPlaceHolderID="phG" runat="Server">
    <px:PXTab ID="tab" runat="server" Width="100%" Height="383px" DataSourceID="ds" DataMember="Persons">
        <Items>
            <px:PXTabItem Text="Persons">
                <Template>
                    <px:PXGrid ID="grid" runat="server" DataSourceID="ds" Width="100%" Height="150px" SkinID="Details" AllowAutoHide="false" SyncPosition="true">
                        <Levels>
                            <px:PXGridLevel DataMember="Persons">
                                <Columns>
                                    <px:PXGridColumn DataField="Name" Width="200" />
                                    <px:PXGridColumn DataField="UserData" Width="200" />
                                </Columns>
                                <RowTemplate>
                                    <px:PXSelector runat="server" ID="CstPXSelector6" DataField="Name"></px:PXSelector>
                                    <px:PXTextEdit runat="server" ID="CstPXTextEdit7" DataField="UserData"></px:PXTextEdit>
                                </RowTemplate>
                            </px:PXGridLevel>
                        </Levels>
                        <AutoSize Container="Window" Enabled="True" MinHeight="150" />
                        <ActionBar>
                        </ActionBar>
                    </px:PXGrid>
                </Template>
            </px:PXTabItem>
        </Items>
        <AutoSize Container="Window" Enabled="True" MinHeight="150" />
    </px:PXTab>
</asp:Content>

