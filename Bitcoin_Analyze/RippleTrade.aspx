<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RippleTrade.aspx.cs" Inherits="Bitcoin_Analyze.RippleTrade" MasterPageFile="~/Site.Master" %>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="Main">
    
         <script type='text/javascript' src='https://cdn.firebase.com/js/client/1.0.11/firebase.js'></script>

    <script type="text/javascript">
        function checkthefirebase(pathkey) {
            

            var dataRef = new Firebase('https://lornestar.firebaseio.com/rippletrade/' + pathkey);
            dataRef.on('value', function (snapshot) {
                //alert('percentage ' + snapshot.val().name);
                var pathkey = snapshot.val().pathkey;
                var percentage = snapshot.val().marketdifference.percentage;
                $("#" + pathkey + "_fee").html(snapshot.val().marketdifference.fee);
                $("#" + pathkey + "_percentage").html(percentage);
                $("#" + pathkey + "_time").html(snapshot.val().marketdifference.time);
                $("#" + pathkey + "_updated").html(snapshot.val().lastupdated);
                $("#" + pathkey + "_starting").html(snapshot.val().marketdifference.starting);
                $("#" + pathkey + "_finish").html(snapshot.val().marketdifference.finish);

                if (percentage < 0) {
                    $("#" + pathkey + "_row").addClass("inthered");
                    $("#" + pathkey + "_row").removeClass("inthegreen");
                }
                else if (percentage > 0) {
                    $("#" + pathkey + "_row").removeClass("inthered");
                    $("#" + pathkey + "_row").addClass("inthegreen");                    
                }
                //$("#" + pathkey + "_percentage").css('font-weight', 'bold');
            });
        }
        
    </script>

    <telerik:RadListView ID="RadListView1" runat="server" ItemPlaceholderID="ListViewContainer" OnNeedDataSource="RadListView1_NeedDataSource">
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
                <td>Starting</td>
                <td>Finish</td>
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
        <td><span id=<%# Eval("pathkey")%>_starting><%# Eval("updated")%></span></td>
        <td><span id=<%# Eval("pathkey")%>_finish><%# Eval("updated")%></span></td>
        </tr>                           
    </ItemTemplate>
</telerik:RadListView>

    </asp:Content>