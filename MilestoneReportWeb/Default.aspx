<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="MilestoneReportWeb.Default" %>

<%@ Register Assembly="DevExpress.Web.v15.2, Version=15.2.16.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Форма мониторинга проектов ИИ</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Button ID="MilestoneReportButton" runat="server" OnClick="MilestoneReportButtonClick" Text="MilestoneReport" Width="275px" />
            <br />
        </div>
        <dx:ASPxGridView ID="MSGridView" ClientInstanceName="MSGridView" runat="server" Theme="Office2010Silver" AutoGenerateColumns="False" Width="100%">
            <SettingsPager Mode="ShowAllRecords">
            </SettingsPager>
            <Settings VerticalScrollBarMode="Visible" HorizontalScrollBarMode="Auto" VerticalScrollableHeight="700" />
            <SettingsBehavior ColumnResizeMode="Control" />
            <Styles>
                <Header HorizontalAlign="Center" Wrap="True">
                    <Paddings PaddingTop="2px" PaddingBottom="2px"></Paddings>
                </Header>
                <Cell Wrap="True" VerticalAlign="Top">
                    <Paddings Padding="5px"></Paddings>
                </Cell>
                <AlternatingRow Enabled="True" Wrap="true"></AlternatingRow>
            </Styles>
        </dx:ASPxGridView>
    </form>
</body>
</html>
