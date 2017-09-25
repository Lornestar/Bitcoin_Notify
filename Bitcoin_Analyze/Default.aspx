<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Bitcoin_Analyze._Default" %>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="Main">
    
    <input type="hidden" id="lastrefresh">

    <script type='text/javascript' src='https://cdn.firebase.com/js/client/1.0.11/firebase.js'></script>

    <script type="text/javascript">
        function checkthefirebase(pathkey) {
            
            $("#lastrefresh").val(new Date().getTime());

            var dataRef = new Firebase('https://lornestar.firebaseio.com/arbitrage/' + pathkey);
            dataRef.on('value', function (snapshot) {
                //alert('percentage ' + snapshot.val().name);
                var currentlastupded = $("#" + pathkey + "_updated").html();
                var newlastupded = snapshot.val().lastupdated;

                //alert(currentlastupded + " " + newlastupded);

                var lastrefresh = parseInt($("#lastrefresh").val());
                var currenttime = new Date().getTime();
                if ((currentlastupded != newlastupded) && (lastrefresh + 30000 < currenttime)) {
                    $("#lastrefresh").val(currenttime);
                    RefreshGrid();
                }

                var pathkey = snapshot.val().pathkey;
                var percentage = snapshot.val().marketdifference.percentage;
                $("#" + pathkey + "_fee").html(snapshot.val().marketdifference.fee);
                $("#" + pathkey + "_percentage").html(percentage);
                $("#" + pathkey + "_time").html(snapshot.val().marketdifference.time);
                $("#" + pathkey + "_updated").html(snapshot.val().lastupdated);
                $("#" + pathkey + "_volume").html(snapshot.val().volume);

                if (percentage < 0) {
                    $("#" + pathkey + "_row").addClass("inthered");
                    $("#" + pathkey + "_row").removeClass("inthegreen");

                    $("#" + pathkey + "_percentage").addClass("inthered");
                    $("#" + pathkey + "_percentage").removeClass("inthegreen");

                    $("#" + pathkey + "_label").addClass("inthered");
                    $("#" + pathkey + "_label").removeClass("inthegreen");
                }
                else if (percentage > 0) {
                    $("#" + pathkey + "_row").removeClass("inthered");
                    $("#" + pathkey + "_row").addClass("inthegreen");

                    $("#" + pathkey + "_percentage").removeClass("inthered");
                    $("#" + pathkey + "_percentage").addClass("inthegreen");

                    $("#" + pathkey + "_label").removeClass("inthered");
                    $("#" + pathkey + "_label").addClass("inthegreen");
                }
                //$("#" + pathkey + "_percentage").css('font-weight', 'bold');

                
                
            });

            
        }

        function RefreshGrid() {
            if ($find("<%= RadGrid1.ClientID %>"))
            {
                var masterTable = $find("<%= RadGrid1.ClientID %>").get_masterTableView();
                masterTable.rebind();
            }
            
        }
        
    </script>

    <telerik:RadListView ID="RadListView1" runat="server" ItemPlaceholderID="ListViewContainer" OnNeedDataSource="RadListView1_NeedDataSource" Visible="false">
    <EmptyDataTemplate>
        <br />
        No Z Report Available on this day
    </EmptyDataTemplate>
    <LayoutTemplate>                                    
        <table cellpadding="5px">
            <tr>
                <td>Name</td>
                <td>Percentage</td>
                <td>Fee</td>
                <td>Time</td>
                <td>Updated</td>
                <td>24 hr Volume</td>
            </tr>
            <tr id="ListViewContainer" runat=server>                
            </tr>
        </table>        
    </LayoutTemplate>                            
    <ItemTemplate>   
    <tr id=<%# Eval("pathkey")%>_row >
         <td><span id=<%# Eval("pathkey")%>_name><%# Eval("name")%></span></td>
         <td><span id=<%# Eval("pathkey")%>_percentage><b><%# Eval("percentage")%></span>%</b></td>
         <td><span id=<%# Eval("pathkey")%>_fee><%# Eval("fee")%></span></td>
         <td><span id=<%# Eval("pathkey")%>_time><%# Eval("time")%></span></td>
         <td><span id=<%# Eval("pathkey")%>_updated><%# Eval("updated")%></span></td>
         <td><span id=<%# Eval("pathkey")%>_volume></span></td>
        </tr>                           
    </ItemTemplate>
</telerik:RadListView>


    <telerik:RadGrid ID="RadGrid1" runat="server" AllowPaging="true" AllowAutomaticInserts="False"  OnNeedDataSource="RadGrid1_NeedDataSource" AllowSorting="true"
                OnItemCommand="RadGrid1_ItemCommand" PageSize=25 AllowFilteringByColumn=true OnItemDataBound="RadGrid1_ItemDataBound">
                <groupingsettings casesensitive="false"></groupingsettings>
                     <MasterTableView Width="100%" CommandItemDisplay="Top" DataKeyNames="arb_results_key" AutoGenerateColumns="False" >
                        <Columns>
                            <telerik:GridBoundColumn DataField="arb_results_key" HeaderText="Path Key" UniqueName="arb_results_key" SortExpression="arb_results_key" CurrentFilterFunction=Contains FilterDelay=1500 Visible="true">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="label" HeaderText="Label" UniqueName="label" CurrentFilterFunction=Contains FilterDelay=1000>
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="starting_node" HeaderText="Starting" UniqueName="starting_node" CurrentFilterFunction="EqualTo" FilterDelay=1000>
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="end_node" HeaderText="End" UniqueName="end_node" CurrentFilterFunction="EqualTo" FilterDelay=1000>
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="percentage" HeaderText="Percentage" UniqueName="percentage" SortExpression="percentage" CurrentFilterFunction=Contains FilterDelay=1000>
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="last_changed" HeaderText="Last Updated" UniqueName="last_changed" SortExpression="last_changed" >
                            </telerik:GridBoundColumn>                            
                            
                        </Columns>
                        <CommandItemSettings ShowAddNewRecordButton=false />
                    </MasterTableView>
                </telerik:RadGrid>

</asp:Content>
