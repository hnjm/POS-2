using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlClient;
using System.Drawing.Printing;

namespace POCS
{
    public partial class FrmViewOrder_SO : Form
    {
        private StringBuilder htmlContent;
        private string newline;
        //string SplInstructions;
        string Currency, strPODate, strRequestor, strVendorName, strVendorAddress, strCnctPerson, strBuyer, strVendorRef, strChargeType, strPODesc, strProjectCC;
        //string DeliveryTerms;
        public FrmViewOrder_SO()
        {
            InitializeComponent();
        }
        private void FrmViewOrder_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'pOS_DBDataSet.PO_Header' table. You can move, or remove it, as needed.
            this.pO_HeaderTableAdapter.FillByType(this.pOCS.PO_Header, "SO");
            webBrowser1.Navigate("about:blank");
            try
            {
                clsGlobals.IESetupMargins();
            }
            catch
            {
                MessageBox.Show("Unable to set Print Margins!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        private void btnView_Click(object sender, EventArgs e)
        {
            Report report = new Report();
            WriteHTML();
            //report.SaveReport(Environment.CurrentDirectory + "\\report.html", htmlContent);
            //webBrowser1.Navigate(Environment.CurrentDirectory + "\\report.html");
            report.SaveReport(Application.StartupPath + "\\report.html", htmlContent);
            webBrowser1.Navigate(Application.StartupPath + "\\report.html");
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            webBrowser1.ShowPrintDialog();
        }

        private void btnPageSetup_Click(object sender, EventArgs e)
        {

            // this.printerPageSetting.ShowPageSetupDialog();     
            webBrowser1.ShowPageSetupDialog();
        }

        private void btnPrintPreview_Click(object sender, EventArgs e)
        {
            webBrowser1.ShowPrintPreviewDialog();
            // SendKeys.Send("%s1");
            //  this.printerPageSetting.ShowPrintPreviewDialog();
        }

        private void WritePageHeader(bool blFormatOne, StringBuilder htmlcontent, string newline)
        {
            if (blFormatOne)
            {
                htmlContent.Append("<table valign=top border='0' width=65% cellpadding='1' cellspacing='1'>" + newline);
                htmlContent.Append("<tr><td colspan=5 class='PONumber' nowrap><span class='OrderNumberTag'> Service Order No:</span><span class='POheader'> SO- " + cboOrders.Text + (chkDraft.Checked ? "<span class='DraftStyle'>Draft" : "") + "</span></td></tr>" + newline);
                htmlContent.Append("<tr> <td valign='top' class='OrderHeaderTag'>" + "SO Date:</td><td colspan=4 class='OrderHeader'>" + strPODate + "<br/>&nbsp;</td></tr>" + newline);
                htmlContent.Append("<tr> <td height=90px width=20%  valign='top' class='OrderHeaderTag'>" + "To:</td><td valign=top colspan=4 class='OrderHeader'><b>" + strVendorName + "</b><br/>" + strVendorAddress + "</td></tr>" + newline);
                htmlContent.Append("<tr><td class='OrderHeaderTag'>" + "Attn: </td><td colspan=4 class='OrderHeader'>" + strCnctPerson + "</td></tr>" + newline);
                htmlContent.Append("<tr> <td valign='top' class='OrderHeaderTag'>" + "Vendor's Ref: </td><td height=30px valign='top' colspan=4 class='OrderHeader'>" + strVendorRef + "</td></tr>" + newline);
                htmlContent.Append("<tr> <td  valign='top' class='OrderHeaderTag'>" + "Requestor: </td><td valign='top' colspan=4 class='OrderHeader'>" + strRequestor + "</td></tr>" + newline);
                htmlContent.Append("<tr> <td valign='top'  class='OrderHeaderTag'>" + "Buyer: </td><td colspan=4 valign='top' class='OrderHeader'>" + strBuyer + "</td></tr>" + newline);
                htmlContent.Append("<tr> <td valign='top'  class='OrderHeaderTag'>" + "Project / CC: </td><td colspan=4 valign='top' class='OrderHeader'>" + strProjectCC + "</td></tr>" + newline);
                htmlContent.Append("<tr> <td valign='top'  class='OrderHeaderTag'>" + "Charge Type: </td><td colspan=4 valign='top' class='OrderHeader'>" + strChargeType + "</td></tr>" + newline);
                htmlContent.Append("<tr> <td valign='top'  class='OrderHeaderTag'>" + "SO Description: &nbsp;</td><td colspan=4 height=30px valign='top' class='OrderHeader'>" + strPODesc + "<br/></td></tr>" + newline);
                htmlContent.Append("</table>" + newline);
            }
            else
            {
                htmlContent.Append("<table valign=top border=0 cellspacing=0 cellpadding=0><tr><td align=left valign=top colspan=5> " + newline);
                htmlContent.Append("<table border='0' align=left valign=top cellpadding=0 cellspacing=0 style='page-break-inside:avoid;border-collapse:collapse;'>" + newline);
                htmlContent.Append("<table  border='0' style='border-collapse:collapse;' width='620px' cellpadding='0' cellspacing='0'>" + newline);
                htmlContent.Append("<td align=center class='OrderHeaderTag' align=center colspan=6 valign=top><u>Service Order Continuation Sheet </u></td></tr>" + newline);
                htmlContent.Append("<tr><td rowspan=5 width='7px'></td><td align=left class='OrderHeaderTag' nowrap> Service Order No:</td>" + newline);
                htmlContent.Append("<td align=left colspan=3><span class='POheader' > SO- " + cboOrders.Text + "</span></td><td rowspan=4 width='70px'></td></tr>" + newline);
                htmlContent.Append("<tr ><td align=left class='OrderHeaderTag'> SO Date:</td>" + newline);
                htmlContent.Append("<td align=left class='OrderHeader' >" + strPODate + "</td>" + newline);
                htmlContent.Append("<td align=left class='OrderHeaderTag'>Requestor: </td>" + newline);
                htmlContent.Append("<td align=left class='OrderHeader'>" + strRequestor + "</td></tr>" + newline);
                htmlContent.Append("<tr ><td class='OrderHeaderTag'> Issued To: </td>" + newline);
                htmlContent.Append("<td class='OrderHeader' width='200px'>" + strVendorName + "</td>" + newline);
                htmlContent.Append("<td class='OrderHeaderTag'>Buyer: </td>" + newline);
                htmlContent.Append("<td class='OrderHeader'>" + strBuyer + "</td></tr>" + newline);
                htmlContent.Append("<tr ><td class='OrderHeaderTag'>Vendor Ref.: </td>" + newline);
                htmlContent.Append("<td class='OrderHeader'>" + strVendorRef + "</td>" + newline);
                // htmlContent.Append("<td class='OrderHeaderTag'>Charge Type:   </td>" + newline);
                // htmlContent.Append("<td class='OrderHeader'>" + strChargeType + "</td></tr>" + newline);
                htmlContent.Append("<td class='OrderHeaderTag'>Project/CC: </td>" + newline);
                htmlContent.Append("<td class='OrderHeader'>" + strProjectCC + "</td>" + newline);
                htmlContent.Append("<tr><td  class='OrderHeaderTag'>SO Description: </td>" + newline);
                htmlContent.Append("<td colspan=4 class='OrderHeader'>" + strPODesc + "</td></tr></table></td></tr></table>" + newline);

            }
        }
        private void WriteItemListHeader(bool blFormatOne, StringBuilder htmlContent, string newline, string strCurrency)
        {
            htmlContent.Append("<table valign=top  border=0 cellpadding=1><tr><td colspan=5> <table  valign=top border='0' style='border-collapse:collapse;table-layout:fixed;' width='100%' cellpadding='0' cellspacing='0'>" + newline);
            htmlContent.Append("<tr class='ItemList'>" + newline);
            htmlContent.Append("<th style='border-collapse:collapse;' width='32px'>&nbsp;Item No</th>" + newline);
            htmlContent.Append("<th width='255px'>Item Description</th>" + newline);
            htmlContent.Append("<th align=center width='50px'>MSR<br/>No</th>" + newline);
            htmlContent.Append("<th width='50px'>Qty</th>" + newline);
            htmlContent.Append("<th align=center width='40px'>Dur.</th>" + newline);
            htmlContent.Append("<th width='45px'>UOM</th>" + newline);
            htmlContent.Append("<th width='74px'>Unit Price" + "<br/>(" + strCurrency + ")" + "</th>" + newline);
            htmlContent.Append("<th width='90px' nowrap>Total Price " + "<br/>(" + strCurrency + ")" + "</th>" + newline);
            htmlContent.Append("</tr>" + newline);
        }
        private void WriteItemsRows(StringBuilder htmlContent, string newline, string Sno, string ChargeCode, string ItemName, string MSRNo, string Period, string Quantity, string UOM, string UnitPrice, string ItemTotal, string GroupNote)
        {
            if (GroupNote != "")
            {
                htmlContent.Append("<tr height='100%' valign='top' class='ItemList'>" + newline);
                htmlContent.Append("<td valign=top   align=center>&nbsp;</td>" + newline);
                htmlContent.Append("<td valign=top ><div ID='mydiv' class='ItemName'><b>" + GroupNote + "<br></b></td>" + newline);
                htmlContent.Append("<td valign=top  align=center>&nbsp;</td>" + newline);
                htmlContent.Append("<td valign=top  align='right'>&nbsp;</td>" + newline);
                htmlContent.Append("<td valign=top  align='right'>&nbsp;</td>" + newline);
                htmlContent.Append("<td valign=top  align=center>&nbsp;</td>" + newline);
                htmlContent.Append("<td valign=top  align='right'>&nbsp;</td>" + newline);
                htmlContent.Append("<td valign=top  align='right'>&nbsp;</td>" + newline);
                htmlContent.Append("</tr>" + newline);
            }
            htmlContent.Append("<tr height='100%' valign='top' class='ItemList'>" + newline);
            htmlContent.Append("<td valign=top   align=center>" + Sno + ".</td>" + newline);
            htmlContent.Append("<td valign=top ><div ID='mydiv' class='ItemName'><span class='costcode'>" + ChargeCode + "</span><br>" + ItemName + "</div></td>" + newline);
            htmlContent.Append("<td valign=top  align=center>" + MSRNo + "</td>" + newline);
            htmlContent.Append("<td valign=top  align='right'>" + Quantity + "</td>" + newline);
            htmlContent.Append("<td valign=top  align=center>" + Period + "</td>" + newline);
            htmlContent.Append("<td valign=top  align=center>" + UOM + "</td>" + newline);
            htmlContent.Append("<td valign=top  align='right'>" + UnitPrice + "</td>" + newline);
            htmlContent.Append("<td valign=top  align='right'>" + ItemTotal + "</td>" + newline);
            htmlContent.Append("</tr>" + newline);
        }

        private void WriteHTML()
        {
            htmlContent = new StringBuilder();
            newline = "\r\n";
            htmlContent.Append("<HTML><HEAD><script type='text/javascript'>function getRowHeight()" + newline);
            htmlContent.Append("{" + newline);
            htmlContent.Append(" var rHeight = document.getElementById('mydiv').clientHeight;" + newline);
            htmlContent.Append("var oVDiv = document.getElementById('mydiv');" + newline);
            htmlContent.Append("//alert ('Height = ' + document.getElementById('DeliveryTerms').clientHeight)" + newline);
            htmlContent.Append("//oVDiv.innerHTML = oVDiv.innerHTML + ' - ' + oVDiv.offsetHeight;" + newline);
            htmlContent.Append("//document.getElementById('PageNo').innerHTML = document.getElementById('PageNo').innerHTML + document.getElementById('lastPage').innerHTML;" + newline);
            htmlContent.Append("var x = document.getElementsByTagName('div');" + newline);
            htmlContent.Append("for (var i=0;i<x.length;i++)" + newline);
            htmlContent.Append("{" + newline);
            htmlContent.Append("if (x[i].className == 'Page')" + newline);
            htmlContent.Append("x[i].innerHTML = x[i].innerHTML + document.getElementById('lastPage').innerHTML;" + newline);
            htmlContent.Append("// if (x[i].className == 'ItemName')" + newline);
            htmlContent.Append("// x[i].innerHTML = x[i].innerHTML + '-' + x[i].offsetHeight;" + newline);
            htmlContent.Append("}" + newline);
            htmlContent.Append("}" + newline);
            htmlContent.Append("</script><TITLE>Report - " + "Service Order" + "</TITLE>" + newline);
            htmlContent.Append("<LINK href='resources/POS.css' rel='stylesheet' type='text/css' media='screen' >" + newline);
            htmlContent.Append("<LINK href='resources/POS_Print.css' rel='stylesheet' type='text/css' media='print' >" + newline);
            htmlContent.Append("</HEAD>" + newline);
            htmlContent.Append("<body  scroll=auto style='text-align: left' valign='top' onLoad='getRowHeight()'>" + newline);
            htmlContent.Append("<table border='0' width='100%' cellpadding=0 cellspacing=0><tr><td><table border='0' width='100%' class='CompanyLogoAddress' >" + newline);
            htmlContent.Append("<tr>" + newline);
            htmlContent.Append("<td width='80%'>&nbsp;</td>" + newline);
            htmlContent.Append("<td style='text-align: right'>" + newline);
            htmlContent.Append("<img src='resources/logo.png' /></td>" + newline);
            htmlContent.Append("</tr>" + newline);
            htmlContent.Append("</table>" + newline);
            StringBuilder sbDeliveryTerms = new StringBuilder();
            string connString = clsGlobals.ConnectionString();
            SqlConnection conn = new SqlConnection(connString);
            string cmdString = "Select * from Po_header where Po_OrderNo = " + Decimal.Parse(cboOrders.Text);
            SqlCommand cmd = new SqlCommand(cmdString, conn);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            //int lc = 0;
            int Page = 1;

            if (reader.Read())
            {
                StringBuilder sbAddress = new StringBuilder();
                sbAddress.Append(reader["VendorAddress"].ToString());
                sbAddress.Replace("\n", "<br/>");
                strVendorName = reader["VendorName"].ToString();
                strVendorAddress = sbAddress.ToString();
                strPODate = String.Format("{0: dd-MMM-yyyy}", reader["PO_Date"]);
                strVendorRef = reader["VendorRef"].ToString();
                strCnctPerson = reader["CnctPerson"].ToString();
                strRequestor = reader["EndUser"].ToString();
                strBuyer = reader["Buyer"].ToString();
                strProjectCC = reader["Project"].ToString();
                strChargeType = reader["ChargeType"].ToString();
                strPODesc = reader["PO_Description"].ToString();
                WritePageHeader(true, htmlContent, newline);
                //SplInstructions = reader["DeliveryTerms"].ToString();
                Currency = reader["Currency"].ToString();
                //sbDeliveryTerms.Append(reader["DeliveryTerms"].ToString());
                Properties.Settings settings = new Properties.Settings();
                sbDeliveryTerms.Append(settings.StdDeliveryInfo);
                sbDeliveryTerms.Replace("this PO", "this SO");
                sbDeliveryTerms.Replace("Payment Terms :", "Payment Terms : " + reader["PaymentTerms"].ToString() + " ");
                sbDeliveryTerms.Replace("Deliver to:", "Deliver to : <b>" + reader["DeliveryPoint"].ToString() + "</b>. ");
                sbDeliveryTerms.Replace("Attn:", "Attn : <b>" + reader["DeliveryPointAttn"].ToString() + "</b> ");
                sbDeliveryTerms.Replace("Promised delivery date :", "Promised delivery date : <b>" + String.Format("{0: dd-MMM-yyyy}", reader["DeliveryDate"]) + "</b> ");
                sbDeliveryTerms.Replace("\n", "<li>");
                WriteItemListHeader(true, htmlContent, newline, Currency);
                //WriteFirstPageItems(htmlContent, newline);
                //Po details
                decimal GrandTotal = 0;
                decimal WithHoldingTotal = 0;
                decimal SubTotal = 0;
                int Sno = 1;
                decimal rowsHeight = 0;
                decimal ItemRows = 0;
                string ItemTotalAppendString, UOMAppendStr, UnitPriceAppendStr;
                StringBuilder sbInI = new StringBuilder();
                sbInI.Append(reader["ImpInstructions"].ToString());
                sbInI.Replace("\n", "<br/>");
                DataSet dsDetails = Common.dsPODetails(Decimal.Parse(cboOrders.Text));

                ItemRows = dsDetails.Tables[0].Rows.Count;
                string UnitPriceAbsolute;
                foreach (DataRow row in dsDetails.Tables["vPOdetailsCalculation"].Rows)
                {
                    UOMAppendStr = String.Format("{0:N2}", row["UOM"]);
                    UnitPriceAbsolute = Convert.ToString(Math.Abs(Convert.ToDouble(row["UnitPrice"])));
                    if (UnitPriceAbsolute.IndexOf('.') == -1 | (UnitPriceAbsolute.IndexOf('.') + 2) == UnitPriceAbsolute.Length)
                    {
                        UnitPriceAppendStr = String.Format("{0:N2}", row["UnitPrice"]);
                    }
                    else
                    {
                        UnitPriceAppendStr = UnitPriceAbsolute;

                    }
                    ItemTotalAppendString = String.Format("{0:N2}", row["ItemValue"]);
                    if (Convert.ToDecimal(row["DiscountValue"]) != 0)
                    {
                        UOMAppendStr = UOMAppendStr + "<br/> Disc. ";
                        UnitPriceAppendStr = UnitPriceAppendStr + " <br/> " + String.Format("{0:F0}", row["Discount"]) + "% ";
                        ItemTotalAppendString = ItemTotalAppendString + " <br/> (" + String.Format("{0:N2}", row["DiscountValue"]) + ")";
                    }
                    if (Convert.ToDecimal(row["WithHolding"]) != 0)
                    {
                        UOMAppendStr = UOMAppendStr + "<br/> W/Tax ";
                        UnitPriceAppendStr = UnitPriceAppendStr + " <br/> " + String.Format("{0:F0}", row["WithHolding"]) + "% ";
                        ItemTotalAppendString = ItemTotalAppendString + " <br/> " + String.Format("{0:N2}", row["WHValue"]);
                    }
                    StringBuilder sbItemDesc = new StringBuilder();
                    StringBuilder sbGroupNote = new StringBuilder();
                    StringBuilder Chargecodes = new StringBuilder();
                    Chargecodes.Append(Common.arrItemChargeCodeDistribution(Convert.ToDecimal(row["po_DetailID"])));
                    if (Chargecodes.Length > 0)
                        Chargecodes.Remove(Chargecodes.Length - 1, 1);
                    Chargecodes = Chargecodes.Replace("100.00%", "");
                    Chargecodes = Chargecodes.Replace("\n", "<br>");
                    decimal decGroupNoteHeight = 0;
                    string ItemDesc = row["ItemDescription"].ToString().TrimEnd(clsUtilies.charsToTrim);
                    sbItemDesc.Append(ItemDesc);
                    sbItemDesc.Replace("\n", "<br>");//+ "***" + rowsHeight + " - " + thisRowHeight.ToString()
                    decimal thisRowHeight = EstimatedRowHeight(sbItemDesc);
                    thisRowHeight += EstimatedRowHeight(Chargecodes);
                    if (row["GroupNote"].ToString() != "")
                    {
                        sbGroupNote.Append(row["GroupNote"].ToString());
                        sbGroupNote.Replace("\n", "<br>");
                        decGroupNoteHeight = EstimatedRowHeight(sbGroupNote);
                        rowsHeight += decGroupNoteHeight;
                    }

                    //WriteItemsRows(htmlContent, newline, Sno.ToString(), row["AccountCode"].ToString(), sbItemDesc.ToString(), row["MSRNo"].ToString(), String.Format("{0:F0}", row["Quantity"]), UOMAppendStr, UnitPriceAppendStr, ItemTotalAppendString, sbGroupNote.ToString());
                    WriteItemsRows(htmlContent, newline, Sno.ToString(), Chargecodes.ToString(), sbItemDesc.ToString(), row["MSRNo"].ToString(), row["Period"].ToString(), String.Format("{0:F0}", row["Quantity"]), UOMAppendStr, UnitPriceAppendStr, ItemTotalAppendString, sbGroupNote.ToString());
                    rowsHeight += thisRowHeight;
                    SubTotal += (Convert.ToDecimal(row["DiscountedPrice"]) + Convert.ToDecimal(row["WHValue"]));
                    WithHoldingTotal += Convert.ToDecimal(row["WHValue"]);
                    GrandTotal += Convert.ToDecimal(row["NetValue"]);

                    if (Page == 1)
                    {
                        if (Sno < ItemRows)
                        {
                            if (rowsHeight <= 350)
                            {
                                //Continue Writing
                            }
                            else
                            {
                                WriteDummyItemRows(htmlContent, newline, 300 - rowsHeight);
                                WriteSpecialIntructions(htmlContent, newline, sbInI.ToString(), WithHoldingTotal, SubTotal, GrandTotal, Currency, false);
                                WriteFooter(htmlContent, newline, sbDeliveryTerms.ToString(), false);
                                WritePageFooter(htmlContent, newline, Page.ToString(), true);
                                Page = Page + 1;
                                WritePageHeader(false, htmlContent, newline);
                                WriteItemListHeader(false, htmlContent, newline, Currency);
                                rowsHeight = 0;
                                //  SubTotal = 0;
                            }
                        }
                        else // Sno = Max
                        {
                            //    SubTotal += (Convert.ToDecimal(row["DiscountedPrice"]) + Convert.ToDecimal(row["WHValue"]));
                            //    WithHoldingTotal += Convert.ToDecimal(row["WHValue"]);
                            //    GrandTotal += Convert.ToDecimal(row["NetValue"]);
                            goto WriteEnd;
                        }
                    }
                    else // Page > 1
                    {
                        if (Sno < ItemRows)
                        {
                            if (rowsHeight <= 540)
                            {
                                //Continue Writing
                            }
                            else
                            {
                                WriteDummyItemRows(htmlContent, newline, 540 - rowsHeight);
                                WriteSpecialIntructions(htmlContent, newline, sbInI.ToString(), WithHoldingTotal, SubTotal, GrandTotal, Currency, false);
                                WriteFooter(htmlContent, newline, sbDeliveryTerms.ToString(), false);
                                WritePageFooter(htmlContent, newline, Page.ToString(), true);
                                Page = Page + 1;
                                WritePageHeader(false, htmlContent, newline);
                                WriteItemListHeader(false, htmlContent, newline, Currency);
                                rowsHeight = 0;
                                // SubTotal = 0;
                            }
                        }
                        else // Sno = Max
                        {
                          //  SubTotal += (Convert.ToDecimal(row["DiscountedPrice"]) + Convert.ToDecimal(row["WHValue"]));
                          //  WithHoldingTotal += Convert.ToDecimal(row["WHValue"]);
                          //  GrandTotal += Convert.ToDecimal(row["NetValue"]);
                            goto WriteEnd;
                        }
                    }
                    Sno = Sno + 1;
                 //   SubTotal += (Convert.ToDecimal(row["DiscountedPrice"]) + Convert.ToDecimal(row["WHValue"]));
                 //   WithHoldingTotal += Convert.ToDecimal(row["WHValue"]);
                 //   GrandTotal += Convert.ToDecimal(row["NetValue"]);
                }
            WriteEnd:
                if (Page == 1)
                {
                    if (rowsHeight <= 350)
                    {
                        WriteDummyItemRows(htmlContent, newline, 330 - rowsHeight);
                    }
                    else if (rowsHeight >= 505)
                    {
                        WriteDummyItemRows(htmlContent, newline, (630 - rowsHeight));

                    }
                }
                else
                {
                    if (rowsHeight < 540)
                        WriteDummyItemRows(htmlContent, newline, 540 - rowsHeight);
                }

                WriteSpecialIntructions(htmlContent, newline, sbInI.ToString(), WithHoldingTotal, SubTotal, GrandTotal, Currency, true);
                WriteFooter(htmlContent, newline, sbDeliveryTerms.ToString(), true);
                WritePageFooter(htmlContent, newline, Page.ToString(), false);
                htmlContent.Append("<div ID='lastPage' style='display:none;'>" + Page + "</div>" + newline);
                htmlContent.Append("</td></tr></table></td></tr></table>" + newline);
                htmlContent.Append("</body>" + newline);
                htmlContent.Append("</html>" + newline);
            }

            if (conn.State == ConnectionState.Open)
                conn.Close();
        }
        private void WriteSpecialIntructions(StringBuilder htmlcontent, string newline, string strImportantInstructions, decimal WithHoldingTotal, decimal SubTotal, decimal GrandTotal, string Currency, bool IsFinalPage)
        {
            htmlContent.Append("<tr  class='ItemListSpl' >" + newline);
            htmlContent.Append("<td colspan=6 height=100% align=center ><span class='INote'> Important Note / Instructions</span></td>" + newline);
            htmlContent.Append("<td rowspan=2 valign='top' align='left' class='cg10'><b>&nbsp;Sub-Total <br/>" + (IsFinalPage ? (WithHoldingTotal > 0 ? "&nbsp; <u>Less:</u><br/>&nbsp; W/Tax </b>" : "") : "&nbsp;") + "</td>" + newline);
            htmlContent.Append("<td rowspan=2 valign='top' align='right' class='NormalStyle'>" + String.Format("{0:N2}", SubTotal) + "<br/><br/>" + (IsFinalPage ? (WithHoldingTotal > 0 ? "(" + String.Format("{0:N2}", WithHoldingTotal) + ")" : "") : "") + "</td></tr>" + newline);
            htmlContent.Append("<tr  class='ItemListSpl'>" + newline);
            htmlContent.Append("<td colspan=6 height='90px' class='INote' valign='top' >" + (IsFinalPage ? strImportantInstructions : " &nbsp; ") + "</td></tr>" + newline);
            htmlContent.Append("<tr  class='ItemListSpl'>" + newline);
            htmlContent.Append("<td valign ='middle' colspan=6 class='cg10'>" + (IsFinalPage ? "<b>" + Currency + " : " + NumberToEnglish.changeCurrencyToWords(Math.Round(GrandTotal, 2), Currency) + ".</b>" : " ") + " </td>" + newline);
            htmlContent.Append("<td height='42px' valign ='middle' class='cg10' ><b>" + (IsFinalPage ? "&nbsp;SO Total</b></td>" : "") + newline);
            htmlContent.Append("<td align='right' valign ='middle' class='NormalStyle' ><b>" + (IsFinalPage ? String.Format("{0:N2}", GrandTotal) : "") + "</b></td>" + newline);
            htmlContent.Append("</tr>" + newline);
        }
        private void WritePageSubTotal(StringBuilder htmlContent, string newline, string PageTotal, string PageNo)
        {
            htmlContent.Append("<tr class='ItemListSpl' valign='top' >" + newline);
            htmlContent.Append("<td colspan=7 align='right' class='cg10'><b>Page Sub-Total</b></td>" + newline);
            htmlContent.Append("<td class='cg10' align='right'><b>" + PageTotal + "</b></td>" + newline);
            htmlContent.Append("</tr></table></td></tr></table>" + newline);
            //htmlContent.Append("<p>&nbsp;</p>" + newline);
            WritePageFooter(htmlContent, newline, PageNo, true);
        }

        private void WriteDummyItemRows(StringBuilder htmlContent, string newline, decimal RemainingHeight)
        {
            htmlContent.Append("<tr valign='top' height='" + RemainingHeight + "px'  class='ItemList'>" + newline);
            htmlContent.Append("<td>&nbsp;</td>" + newline);
            htmlContent.Append("<td>&nbsp;</td>" + newline);
            htmlContent.Append("<td></td>" + newline);
            htmlContent.Append("<td></td>" + newline);
            htmlContent.Append("<td>&nbsp;</td>" + newline);
            htmlContent.Append("<td>&nbsp;</td>" + newline);
            htmlContent.Append("<td>&nbsp;</td>" + newline);
            htmlContent.Append("<td>&nbsp;</td>" + newline);
            htmlContent.Append("</tr>" + newline);
            //rowsHeight = rowsHeight + 30;  
        }
        private string GetFormatUnitPrice(decimal UPrice)
        {
            /* switch (UPrice)
             {
                 case Math.Floor(UPrice):
                     //dostuff
                     break;
                 default:
                     //dostuff;
                     break;
             }*/
            return UPrice.ToString();
        }
        private void WriteFooter(StringBuilder htmlContent, string newline, string strDeliveryTerms, bool IsLastPage)
        {
            if (IsLastPage)
            {
                htmlContent.Append("<tr class='ItemListSpl' > <td rowspan=2 colspan=4 class='DeliveryTerms' id='DeliveryTerms' valign=top><ul><li>" + strDeliveryTerms + "</ul> </td>" + newline);
                htmlContent.Append("<td valign=top colspan=4 class='cg10' > &nbsp; For and on behalf of <br/>&nbsp;<b> " + clsGlobals.CompanyName() + newline);
                htmlContent.Append("<br/>" + newline);
                htmlContent.Append("<br/>" + newline);
                htmlContent.Append("<br/></td></tr>" + newline);
                htmlContent.Append("<tr class='ItemListSpl'><td valign=top colspan=4  class='cg10' >&nbsp;Authorised Signatory</td></tr>" + newline);
            }
            else
            {
                htmlContent.Append("<tr class='ItemListSpl' > <td rowspan=2 colspan=8 class='DeliveryTerms' align=center height= '90px' valign='middle'>-Continued Next Page-</td>" + newline);
            }
            htmlContent.Append("</table></td></tr></table>");
        }
        private void WritePageFooter(StringBuilder htmlContent, string newline, string PageNo, bool blDividePage)
        {
            htmlContent.Append("<table border=0 width='100%' cellspacing=0 cellpadding=0><tr><td align='center' class='Footer'>1-Vendor Original, 2-Master Control Copy, 3-Accounts Copy, 4-Requisitioner's Copy<br/><div class='Page' ID='PageNo'>Pg " + PageNo + " of </td></tr></table>" + newline);
            htmlContent.Append((blDividePage ? "<p style='page-break-before:always; text-align:left; line-height:1px;'>&nbsp;" : "") + newline);
            if (!blDividePage | PageNo != "1")
                htmlContent.Append("</p>" + newline);
            //htmlContent.Append("" + newline);
        }
        private decimal EstimatedRowHeight(StringBuilder strItemDesc)
        {
            decimal rHeight, quotient, remainder, whole;
            int intStringLength;
            // For example, round 18 to the nearest interval of 5
            // Quotient = 3.6, Remainder = 0.6, Whole = 3
            string strWord = strItemDesc.ToString();
            string[] Words = strWord.Split('\n');
            whole = 0;
            foreach (string Line in Words)
            {
                intStringLength = Line.Length;
                quotient = decimal.Divide(intStringLength, 45);
                remainder = decimal.Subtract(quotient, decimal.Floor(quotient));
                whole = decimal.Subtract(quotient, remainder);
                whole++;
            }
            rHeight = decimal.Multiply(whole, 15);
            return rHeight;
        }

    }
}