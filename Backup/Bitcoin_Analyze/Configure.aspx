<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Configure.aspx.cs" Inherits="Bitcoin_Analyze.Configure" MasterPageFile="~/Site.master"%>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="Main">

<center>

<table>
    <tr>
        <td>
        
        <telerik:RadGrid ID="RadGrid1" runat="server" AllowPaging="true" AllowAutomaticInserts="False" AllowAutomaticUpdates="True" AllowSorting="true" 
                          AllowAutomaticDeletes="True" PageSize=50 Width=600px
                          OnNeedDataSource="RadGrid1_NeedDataSource" OnItemCommand="RadGrid1_ItemCommand" ShowFooter=false>
                         <MasterTableView Width="100%" DataKeyNames="path_key" AutoGenerateColumns="False" InsertItemPageIndexAction="ShowItemOnCurrentPage" CommandItemDisplay="Top" Name="Paths" >                         
                            <Columns>                                                                
                                <telerik:GridBoundColumn DataField="path_key" HeaderText="path_key" SortExpression="path_key" UniqueName="path_key" ReadOnly=true >                     
                                </telerik:GridBoundColumn>      
                                <telerik:GridBoundColumn DataField="path_name" HeaderText="Name" SortExpression="path_name" UniqueName="path_name" ReadOnly=true >
                                </telerik:GridBoundColumn>                                      
                                <telerik:GridBoundColumn DataField="page_order" HeaderText="Order" SortExpression="page_order" UniqueName="page_order" >
                                </telerik:GridBoundColumn>      
                                <telerik:GridButtonColumn ButtonType=LinkButton Text="Route" CommandName="Route" ItemStyle-Width="100px" ItemStyle-HorizontalAlign=Center ></telerik:GridButtonColumn>
                                <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn1">
                                            <HeaderStyle Width="20px"></HeaderStyle>
                                            <ItemStyle CssClass="MyImageButton"></ItemStyle>
                                </telerik:GridEditCommandColumn>
                                <telerik:GridButtonColumn Text="Delete" CommandName="Delete" ButtonType="ImageButton" ConfirmText="Are you sure?" ItemStyle-Width=30/>
                            </Columns>                           
                            <SortExpressions>
                                <telerik:GridSortExpression FieldName="page_order" SortOrder=Ascending />
                            </SortExpressions>
                            <CommandItemSettings ShowAddNewRecordButton=true ShowExportToExcelButton=false AddNewRecordText="Insert Path"/>
                            <EditFormSettings ColumnNumber="2" CaptionDataField="path_key" InsertCaption="New Path">
                                            <FormTableItemStyle Wrap="False"></FormTableItemStyle>
                                            <FormCaptionStyle CssClass="EditFormHeader"></FormCaptionStyle>
                                            <FormMainTableStyle GridLines="None" CellSpacing="0" CellPadding="1"></FormMainTableStyle>
                                            <EditColumn ButtonType=PushButton InsertText="Insert Path" UniqueName="EditCommandColumn1" CancelText="Cancel">                                                                               </EditColumn>
                                            <FormTableButtonRowStyle HorizontalAlign="Right" CssClass="EditFormButtonRow"></FormTableButtonRowStyle>                                    
                                        </EditFormSettings>
                         </MasterTableView>
                         <GroupingSettings CaseSensitive=false />
                         <ClientSettings EnableRowHoverStyle="True">
                            <Selecting AllowRowSelect=true />
                         </ClientSettings>
        </telerik:RadGrid>

        </td>
        <td>

        <asp:Label ID=lblpath runat=server></asp:Label>
        <asp:HiddenField ID=hdpathkey runat=server Value=0 />
            <telerik:RadGrid ID="RadGrid2" runat="server" AllowPaging="true" AllowAutomaticInserts="False" AllowAutomaticUpdates="True" AllowSorting="false" 
                          AllowAutomaticDeletes="True" PageSize=50  Width=400px OnItemDataBound="RadGrid2_ItemDataBound"
                          OnNeedDataSource="RadGrid2_NeedDataSource" OnItemCommand="RadGrid2_ItemCommand" ShowFooter=false>
                         <MasterTableView Width="100%" DataKeyNames="path_route_key,path_key,market_key" AutoGenerateColumns="False" InsertItemPageIndexAction="ShowItemOnCurrentPage" CommandItemDisplay="Top" Name="Paths" >                         
                            <Columns>                                                                
                                <telerik:GridBoundColumn DataField="path_route_key" HeaderText="path_route_key" SortExpression="path_route_key" UniqueName="path_route_key" ReadOnly=true Visible=false>                     
                                </telerik:GridBoundColumn>      
                                <telerik:GridBoundColumn DataField="path_key" HeaderText="path_key" SortExpression="path_key" UniqueName="path_key" ReadOnly=true >
                                </telerik:GridBoundColumn>                                      
                                <telerik:GridBoundColumn DataField="market_key" HeaderText="market_key" SortExpression="market_key" UniqueName="market_key" ReadOnly=true>
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="market_shortname" HeaderText="market_shortname" SortExpression="market_shortname" UniqueName="market_shortname" ReadOnly=true>                                
                                </telerik:GridBoundColumn>                                
                                <telerik:GridTemplateColumn HeaderText="market_key2" DataField=market_key2 UniqueName=market_key2 Visible=false>
                                    <InsertItemTemplate>     
                                        <telerik:RadComboBox ID=ddlmarket runat=server EmptyMessage="Select Market" Width=300                             
                            HighlightTemplatedItems="true">
                                             <HeaderTemplate>
                                                <table>
                                                    <tr>
                                                        <td>key</td>
                                                        <td>name</td>
                                                    </tr>
                                                </table>                                                     
                                           </HeaderTemplate>
                                           <ItemTemplate>
                                            <table>
                                                <tr>
                                                    <td><%# DataBinder.Eval(Container.DataItem, "market_key") %></li></td>
                                                    <td><%# DataBinder.Eval(Container.DataItem, "market_shortname") %></li></td>
                                                </tr>
                                            </table>
                                           </ItemTemplate>
                                        </telerik:RadComboBox>                 
                                    </InsertItemTemplate>
                                    <EditItemTemplate>
                                       <telerik:RadComboBox ID=ddlmarket runat=server EmptyMessage="Select Market" Width=300                             
                            HighlightTemplatedItems="true">
                                             <HeaderTemplate>
                                                <table>
                                                    <tr>
                                                        <td>key</td>
                                                        <td>name</td>
                                                    </tr>
                                                </table>                                                     
                                           </HeaderTemplate>
                                           <ItemTemplate>
                                            <table>
                                                <tr>
                                                    <td><%# DataBinder.Eval(Container.DataItem, "market_key") %></li></td>
                                                    <td><%# DataBinder.Eval(Container.DataItem, "market_shortname") %></li></td>
                                                </tr>
                                            </table>
                                           </ItemTemplate>
                                        </telerik:RadComboBox>
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn1">
                                            <HeaderStyle Width="20px"></HeaderStyle>
                                            <ItemStyle CssClass="MyImageButton"></ItemStyle>
                                </telerik:GridEditCommandColumn>
                                <telerik:GridButtonColumn Text="Delete" CommandName="Delete" ButtonType="ImageButton" ConfirmText="Are you sure?" ItemStyle-Width=30/>
                            </Columns>                                                       
                            <CommandItemSettings ShowAddNewRecordButton=true ShowExportToExcelButton=false AddNewRecordText="Insert Path"/>
                            <EditFormSettings ColumnNumber="2" CaptionDataField="path_route_key" InsertCaption="New Path">
                                            <FormTableItemStyle Wrap="False"></FormTableItemStyle>
                                            <FormCaptionStyle CssClass="EditFormHeader"></FormCaptionStyle>
                                            <FormMainTableStyle GridLines="None" CellSpacing="0" CellPadding="1"></FormMainTableStyle>
                                            <EditColumn ButtonType=PushButton InsertText="Insert Path" UniqueName="EditCommandColumn1" CancelText="Cancel">                                                                               </EditColumn>
                                            <FormTableButtonRowStyle HorizontalAlign="Right" CssClass="EditFormButtonRow"></FormTableButtonRowStyle>                                    
                                        </EditFormSettings>
                         </MasterTableView>
                         <GroupingSettings CaseSensitive=false />
                         <ClientSettings EnableRowHoverStyle="True">
                            <Selecting AllowRowSelect=true />
                         </ClientSettings>
        </telerik:RadGrid>
            
        </td>
    </tr>
</table>

        </center>

</asp:Content>