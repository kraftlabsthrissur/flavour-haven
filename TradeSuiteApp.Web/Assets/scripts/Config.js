Masters = {
    get_item: function (release) {

        Table = 'ItemName';
        $.ajax({
            url: '/Masters/Item/GetItemsAutoComplete',
            data: {
                Hint: $('#ItemName').val(),
                Type: $('#search-item-type').val(),
                ItemCategoryID: $("#ItemCategoryID").val()
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });

    },
    get_treatment_medicine: function (release) {

        Table = 'ItemName';
        $.ajax({
            url: '/Masters/Item/GetTreatmentItemsAutoComplete',
            data: {
                Hint: $('#Medicine').val(),
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });

    },

    get_lab_test: function (release) {

        Table = 'ItemName';
        $.ajax({
            url: '/AHCMS/IPCaseSheet/GetLabTestAutoComplete',
            data: {
                Hint: $('#LabTest').val(),
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });

    },

    get_treatment_medicineList: function (release) {

        Table = 'ItemName';
        $.ajax({
            url: '/Masters/Item/GetTreatmentItemsAutoComplete',
            data: {
                Hint: $('#Medicines').val(),
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });

    },

    get_internal_medicineList: function (release) {

        Table = 'ItemName';
        $.ajax({
            url: '/Masters/Item/GetTreatmentItemsAutoComplete',
            data: {
                Hint: $('#Medicines').val(),
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });

    },

    get_item_code_from: function (release) {
        Table = 'ItemCode';
        $.ajax({
            url: '/Reports/Sales/GetAutoComplete',
            data: {
                Term: $('#ItemCodeFrom').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },

    get_item_code_to: function (release) {
        Table = 'ItemCode';
        $.ajax({
            url: '/Reports/Sales/GetAutoComplete',
            data: {
                Term: $('#ItemCodeTo').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },

    get_stock_items: function (release) {
        $.ajax({
            url: '/Masters/Item/GetStockableItemsForAutoComplete',
            data: {
                Hint: $('#ItemName').val(),
                ItemCategoryID: $("#ItemCategoryID").val(),
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },

    get_receipt_no_from: function (release) {
        var Table = "StockReceiptNo";
        $.ajax({
            url: '/Reports/Stock/GetAutoComplete',
            data: {
                Term: $('#ReceiptNoFrom').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },

    get_receipt_no_to: function (release) {
        var Table = "StockReceiptNo";
        $.ajax({
            url: '/Reports/Stock/GetAutoComplete',
            data: {
                Term: $('#ReceiptNoTo').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },

    get_issue_no_from: function (release) {
        var Table = "StockIssueNo";
        $.ajax({
            url: '/Reports/Stock/GetAutoComplete',
            data: {
                Term: $('#IssueNoFrom').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },

    get_issue_no_to: function (release) {
        var Table = "StockIssueNo";
        $.ajax({
            url: '/Reports/Stock/GetAutoComplete',
            data: {
                Term: $('#IssueNoTo').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },

    get_batchNo: function (release) {
        var Table = "Batch";
        $.ajax({
            url: '/Reports/Stock/GetAutoComplete',
            data: {
                Term: $('#BatchNo').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },

    get_customer: function (release) {
        $.ajax({
            url: '/Masters/Customer/GetCustomersAutoComplete',
            data: {
                Hint: $('#CustomerName').val(),
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },

    get_customer_code: function (release) {
        Table = 'CustomerCode';
        $.ajax({
            url: '/Reports/Sales/GetAutoComplete',
            data: {
                Term: $('#CustomerCodeFrom').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },

    get_customer_code_from: function (release) {
        Table = 'CustomerCode';
        $.ajax({
            url: '/Reports/Sales/GetAutoComplete',
            data: {
                Term: $('#CustomerCodeFrom').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },

    get_customer_code_to: function (release) {
        Table = 'CustomerCode';
        $.ajax({
            url: '/Reports/Sales/GetAutoComplete',
            data: {
                Term: $('#CustomerCodeTo').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },

    get_invoice_no_from: function (release) {
        var Table = "SalesInvoiceNo";
        $.ajax({
            url: '/Reports/Stock/GetAutoComplete',
            data: {
                Term: $('#InvoiceNoFrom').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },

    get_invoice_no_to: function (release) {
        var Table = "SalesInvoiceNo";
        $.ajax({
            url: '/Reports/Stock/GetAutoComplete',
            data: {
                Term: $('#InvoiceNoTo').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },

    get_employees: function (release) {
        $.ajax({
            url: '/Masters/Employee/GetEmployeeForAutoComplete',
            data: {
                Hint: $('#EmployeeName').val(),
                offset: 0,
                limit: 0
            },
            dataType: "json",
            type: "POST",
            success: function (result) {
                release(result.data);
            }
        });
    },

    get_sales_order_no_from: function (release) {
        Table = 'SalesOrderNo';
        $.ajax({
            url: '/Reports/Sales/GetAutoComplete',
            data: {
                Term: $('#SalesOrderNoFrom').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },

    get_sales_order_no_to: function (release) {
        Table = 'SalesOrderNo';
        $.ajax({
            url: '/Reports/Sales/GetAutoComplete',
            data: {
                Term: $('#SalesOrderNoTo').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },

    get_tds_transaction_no: function (release) {
        Table = 'TDSTransactionNo';
        $.ajax({
            url: '/Reports/Accounts/GetAutoComplete',
            data: {
                Term: $('#TransactionNo').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },

    get_supplier: function (release) {
        $.ajax({
            url: '/Masters/Supplier/GetAllSupplierAutoComplete',
            data: {
                Term: $('#SupplierName').val(),
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },

    get_tds_code: function (release) {
        Table = 'TDSCode';
        $.ajax({
            url: '/Reports/Accounts/GetAutoComplete',
            data: {
                Term: $('#TDSCode').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },

    get_pan_no: function (release) {
        Table = 'PanNo';
        $.ajax({
            url: '/Reports/Accounts/GetAutoComplete',
            data: {
                Term: $('#PanNo').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },

    get_preprocess_issue_item_AutoComplete: function (release) {
        $.ajax({
            url: '/Masters/Item/GetPreProcessIssueItemsForAutoComplete',
            data: {
                Hint: $('#IssueItemName').val(),
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },

    get_production_groupname: function (release) {
        Table = 'ProductionGroup';
        $.ajax({
            url: '/Reports/Manufacturing/GetAutoComplete',
            data: {
                Term: $('#ProductionGroupName').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },

    get_production_batchno: function (release) {

        Table = 'ProductionBatchNo';
        $.ajax({
            url: '/Reports/Manufacturing/GetAutoComplete',
            data: {
                Term: $('#BatchNo').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },

    get_preprocess_issue_no: function (release) {
        var Table = "PurificationissueNo";
        $.ajax({
            url: '/Reports/Manufacturing/GetAutoComplete',
            data: {
                Term: $('#IssueNo').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },

    get_preprocess_receipt_no: function (release) {
        var Table = "PurificationReceiptNo";
        $.ajax({
            url: '/Reports/Manufacturing/GetAutoComplete',
            data: {
                Term: $('#ReceiptNo').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },

    get_preprocess_receipt_item_AutoComplete: function (release) {
        $.ajax({
            url: '/Masters/Item/GetPreProcessReceiptItemForAutoComplete',
            data: {
                Hint: $('#ReceiptItemName').val(),
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },

    get_production_group: function (release) {
        var Hint = $('#ProductionGroupName').val();
        $.ajax({
            url: '/Masters/Item/GetProductionGroupItemAutoComplete',
            data: {
                NameHint: Hint,
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data.data);
            }
        });
    },

    get_production_groups: function (release) {
        var Hint = $('#ProductionGroupName').val();
        $.ajax({
            url: '/Masters/Item/GetProductionGroupItemAutoCompleteForReport',
            data: {
                NameHint: Hint,
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data.data);
            }
        });
    },

    get_doctor: function (release) {
        $.ajax({
            url: '/Masters/Supplier/GetDoctorAutoComplete',
            data: {
                term: $('#DoctorName').val(),
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },

    get_fsoname: function (release) {
        $.ajax({
            url: '/Masters/Employee/GetEmployeeForAutoComplete',
            data: {
                Hint: $('#FSOName').val(),
                offset: 0,
                limit: 0
            },
            dataType: "json",
            type: "POST",
            success: function (result) {
                release(result.data);
            }
        });
    },

    get_pono_from: function (release) {
        //var type = $(".order-type:checked").val();
        var Table;
        //if (type == "Stock") {
        Table = 'PurchaseOrder';
        //}
        //else if (type == "Service") {
        //    Table = 'PurchaseOrderForService';
        //}
        $.ajax({
            url: '/Reports/Purchase/GetAutoComplete',
            data: {
                Term: $('#PONOFrom').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },

    get_pono_to: function (release) {
        //var type = $(".order-type:checked").val();
        var Table;
        //if (type == "Stock") {
        Table = 'PurchaseOrder';
        //}
        //else if (type == "Service") {
        //   Table = 'PurchaseOrderForService';
        //}
        $.ajax({
            url: '/Reports/Purchase/GetAutoComplete',
            data: {
                Term: $('#PONOTo').val(),
                Table: Table
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },

    get_treatment: function (release) {

        Table = 'ItemName';
        $.ajax({
            url: '/Masters/Treatment/GetTreatmentAutoComplete',
            data: {
                Hint: $('#TreatmentName').val(),
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });

    },

    get_physiotherapy_item: function (release) {

        Table = 'ItemName';
        $.ajax({
            url: '/AHCMS/IPCaseSheet/GetPhysiotherapyAutoComplete',
            data: {
                Hint: $('#Physiotherapy').val(),
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });

    },

    get_xray_item: function (release) {

        Table = 'ItemName';
        $.ajax({
            url: '/AHCMS/IPCaseSheet/GetXrayAutoComplete',
            data: {
                Hint: $('#XrayName').val(),
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });

    },

    get_patient: function (release) {
        $.ajax({
            url: '/Masters/InternationalPatient/GetPatientAutoComplete',
            data: {
                Hint: $('#PatientName').val(),
                offset: 0,
                limit: 0
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },

    get_treatment_room: function (release) {
        $.ajax({
            url: '/Masters/TreatmentRoom/GetTreatmentRoomAutoComplete',
            data: {
                Hint: $('#TreatmentRoom').val(),
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });

    },

    get_therapist_list: function (release) {
        $.ajax({
            url: '/Masters/Employee/GetTherapistAutoComplete',
            data: {
                Hint: $('#Therapist').val(),
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });

    },

    get_nursing_station: function (release) {
        $.ajax({
            url: '/Masters/WareHouse/GetNursingStationAutoComplete',
            data: {
                Hint: $('#NursingStation').val(),
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });

    },
    get_warehouse: function (release) {
        $.ajax({
            url: '/Masters/WareHouse/GetNursingStationAutoComplete',
            data: {
                Hint: $('#WareHouseTo').val(),
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
    get_account_group: function (release) {
        $.ajax({
            url: '/Masters/AccountGroup/GetAccountGroupParentAutoComplete',
            data: {
                Hint: $('#AccountGroupName').val(),
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        })
    },
    get_account_head: function (release) {
        $.ajax({
            url: '/Masters/AccountHead/GetAccountHeadListAutoCompleteV3',
            data: {
                Hint: $('#AccountHead').val(),
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        })
    },
    get_Appointment_Scheduled_patient: function (release) {
        $.ajax({
            url: '/Masters/InternationalPatient/AppointmentScheduledPatientAutoComplete',
            data: {
                Hint: $('#Patient').val()
            },
            dataType: "json",
            type: "POST",
            success: function (data) {
                release(data);
            }
        });
    },
}
Config = {
    item_name: {
        'source': Masters.get_item,
        'minLength': 1,
        'template': '<ul class="uk-nav uk-nav-autocomplete uk-autocomplete-results">{{~items}}<li data-t="44" data-value="{{$item.ItemName}} ({{$item.ItemCode}})"  data-id="{{$item.ItemID}}"><a>{{$item.ItemName}} ({{$item.ItemCode}})</a></li>{{/items}}</ul>'
    },

    item_code_from: {
        'source': Masters.get_item_code_from,
        'minLength': 1,
        'template': '<ul class="uk-nav uk-nav-autocomplete uk-autocomplete-results">{{~items}}<li data-value="{{$item.Code}}"  data-id="{{$item.ID}}"><a>{{$item.Code}}</a></li>{{/items}}</ul>'
    },

    item_code_to: {
        'source': Masters.get_item_code_to,
        'minLength': 1,
        'template': '<ul class="uk-nav uk-nav-autocomplete uk-autocomplete-results">{{~items}}<li data-value="{{$item.Code}}"  data-id="{{$item.ID}}"><a>{{$item.Code}}</a></li>{{/items}}</ul>'
    },

    stock_items: {
        'source': Masters.get_stock_items,
        'minLength': 1,
        'template': '<ul class="uk-nav uk-nav-autocomplete uk-autocomplete-results">'
            + '{{~items}}'
            + '<li data-value="{{$item.ItemName}}" data-name="{{$item.ItemName}}"   data-id="{{$item.ItemID}}">'
            + '<a>{{$item.ItemName}} ({{$item.Code}})</a>'
            + '</li>'
            + '{{/items}}'
            + '</ul>'
    },

    stock_receipt_no_from: {
        'source': Masters.get_receipt_no_from,
        'minLength': 1,
        'template': '<ul class="uk-nav uk-nav-autocomplete uk-autocomplete-results">{{~items}}<li data-value="{{$item.Code}}"  data-id="{{$item.ID}}"><a>{{$item.Code}}</a></li>{{/items}}</ul>'
    },

    stock_receipt_no_to: {
        'source': Masters.get_receipt_no_to,
        'minLength': 1,
        'template': '<ul class="uk-nav uk-nav-autocomplete uk-autocomplete-results">{{~items}}<li data-value="{{$item.Code}}"  data-id="{{$item.ID}}"><a>{{$item.Code}}</a></li>{{/items}}</ul>'
    },

    stock_issue_no_from: {
        'source': Masters.get_issue_no_from,
        'minLength': 1,
        'template': '<ul class="uk-nav uk-nav-autocomplete uk-autocomplete-results">{{~items}}<li data-value="{{$item.Code}}"  data-id="{{$item.ID}}"><a>{{$item.Code}}</a></li>{{/items}}</ul>'
    },

    stock_issue_no_to: {
        'source': Masters.get_issue_no_to,
        'minLength': 1,
        'template': '<ul class="uk-nav uk-nav-autocomplete uk-autocomplete-results">{{~items}}<li data-value="{{$item.Code}}"  data-id="{{$item.ID}}"><a>{{$item.Code}}</a></li>{{/items}}</ul>'
    },

    batch_no: {
        'source': Masters.get_batchNo,
        'minLength': 1,
        'template': '<ul class="uk-nav uk-nav-autocomplete uk-autocomplete-results">'
            + '{{~items}}'
            + '<li data-value="{{ $item.Code }}" data-id="{{$item.ID}}">'
            + '<a>{{ $item.Code }}</a>'
            + '</li>'
            + '{{/items}}'
            + '</ul>'
    },

    customer_name: {
        'source': Masters.get_customer,
        'minLength': 1,
        'template': '<ul class="uk-nav uk-nav-autocomplete uk-autocomplete-results">'
            + '{{~items}}'
            + '<li data-value="{{$item.Name}}"  data-id="{{$item.ID}}">'
            + '<a>{{$item.Name}} ({{$item.Code}})</a>'
            + '</li>'
            + '{{/items}}'
            + '</ul>'
    },

    customer_code: {
        'source': Masters.get_customer_code,
        'minLength': 1,
        'template': '<ul class="uk-nav uk-nav-autocomplete uk-autocomplete-results">{{~items}}<li data-value="{{$item.Code}}"  data-id="{{$item.ID}}"><a>{{$item.Code}}</a></li>{{/items}}</ul>'
    },

    customer_code_from: {
        'source': Masters.get_customer_code_from,
        'minLength': 1,
        'template': '<ul class="uk-nav uk-nav-autocomplete uk-autocomplete-results">{{~items}}<li data-value="{{$item.Code}}"  data-id="{{$item.ID}}"><a>{{$item.Code}}</a></li>{{/items}}</ul>'
    },

    customer_code_to: {
        'source': Masters.get_customer_code_to,
        'minLength': 1,
        'template': '<ul class="uk-nav uk-nav-autocomplete uk-autocomplete-results">{{~items}}<li data-value="{{$item.Code}}"  data-id="{{$item.ID}}"><a>{{$item.Code}}</a></li>{{/items}}</ul>'
    },

    invoice_no_from: {
        'source': Masters.get_invoice_no_from,
        'minLength': 1,
        'template': '<ul class="uk-nav uk-nav-autocomplete uk-autocomplete-results">{{~items}}<li data-value="{{$item.Code}}"  data-id="{{$item.ID}}"><a>{{$item.Code}}</a></li>{{/items}}</ul>'
    },

    invoice_no_to: {
        'source': Masters.get_invoice_no_to,
        'minLength': 1,
        'template': '<ul class="uk-nav uk-nav-autocomplete uk-autocomplete-results">{{~items}}<li data-value="{{$item.Code}}"  data-id="{{$item.ID}}"><a>{{$item.Code}}</a></li>{{/items}}</ul>'
    },

    employee_name: {
        'source': Masters.get_employees,
        'minLength': 1,
        'template': '<ul class="uk-nav uk-nav-autocomplete uk-autocomplete-results">'
        + '{{~items}}'
        + '<li data-value="{{ $item.Name}}"data-id="{{$item.EmployeeID}}"data-EmployeeCode="{{$item.Code}}"data-placement="{{$item.Place}}">'
        + '<a>{{ $item.Name }}</a>'
        + '</li>'
        + '{{/items}}'
        + '</ul>'
    },

    sales_order_no_from: {
        'source': Masters.get_sales_order_no_from,
        'minLength': 1,
        'template': '<ul class="uk-nav uk-nav-autocomplete uk-autocomplete-results">{{~items}}<li data-value="{{$item.Code}}"  data-id="{{$item.ID}}"><a>{{$item.Code}}</a></li>{{/items}}</ul>'
    },

    sales_order_no_to: {
        'source': Masters.get_sales_order_no_to,
        'minLength': 1,
        'template': '<ul class="uk-nav uk-nav-autocomplete uk-autocomplete-results">{{~items}}<li data-value="{{$item.Code}}"  data-id="{{$item.ID}}"><a>{{$item.Code}}</a></li>{{/items}}</ul>'
    },

    tds_transaction_no: {
        'source': Masters.get_tds_transaction_no,
        'minLength': 1,
        'template': '<ul class="uk-nav uk-nav-autocomplete uk-autocomplete-results">{{~items}}<li data-value="{{$item.Code}}"  data-id="{{$item.ID}}"><a>{{$item.Code}}</a></li>{{/items}}</ul>'
    },

    supplier_name: {
        'source': Masters.get_supplier,
        'minLength': 1,
        'template': '<ul class="uk-nav uk-nav-autocomplete uk-autocomplete-results">'
        + '{{~items}}'
        + '<li data-value="{{ $item.Name }}" data-id="{{$item.ID}}"data-location="{{$item.Location}}">'
        + '<a> {{ $item.Name }} ({{{ $item.Code }}}) <div>{{{ $item.Location }}}</div></a>'
        + '</li>'
        + '{{/items}}'
        + '</ul>'
    },

    tds_code: {
        'source': Masters.get_tds_code,
        'minLength': 1,
        'template': '<ul class="uk-nav uk-nav-autocomplete uk-autocomplete-results">{{~items}}<li data-value="{{$item.Code}}"  data-id="{{$item.ID}}"><a>{{$item.Code}}</a></li>{{/items}}</ul>'
    },

    pan_no: {
        'source': Masters.get_pan_no,
        'minLength': 1,
        'template': '<ul class="uk-nav uk-nav-autocomplete uk-autocomplete-results">{{~items}}<li data-value="{{$item.Code}}"  data-id="{{$item.ID}}"><a>{{$item.Code}}</a></li>{{/items}}</ul>'
    },
    preprocess_issue_item: {
        'source': Masters.get_preprocess_issue_item_AutoComplete,
        'minLength': 1,
        'template': '<ul class="uk-nav uk-nav-autocomplete uk-autocomplete-results">'
        + ' {{~items}}'
        + '<li data-value="{{ $item.Name }}" data-itemid="{{$item.Id}}">'
        + '<a> {{ $item.Name }} ({{{ $item.Unit }}}) <div>{{{ $item.Activity }}}</div> </a>'
        + '</li>'
        + '{{/items}}'
        + '</ul>'
    },

    production_group_name: {
        'source': Masters.get_production_groupname,
        'minLength': 1,
        'template': '<ul class="uk-nav uk-nav-autocomplete uk-autocomplete-results">'
         + '{{~items}}'
         + '<li data-value="{{ $item.Code }}"data-id="{{$item.ID}}">'
         + '<a>{{ $item.Code }}</a>'
         + '</li>'
         + '{{/items}}'
         + '</ul>'
    },

    production_batch_no: {
        'source': Masters.get_production_batchno,
        'minLength': 1,
        'template': '<ul class="uk-nav uk-nav-autocomplete uk-autocomplete-results">'
        + '{{~items}}'
        + '<li data-value="{{ $item.Code }}" data-id="{{$item.ID}}">'
        + '<a> {{ $item.Code }} </a>'
        + '</li>'
        + '{{/items}}</ul>'
    },

    preprocess_issue_no: {
        'source': Masters.get_preprocess_issue_no,
        'minLength': 1,
        'template': '<ul class="uk-nav uk-nav-autocomplete uk-autocomplete-results">'
        + '{{~items}}'
        + '<li data-value="{{ $item.Code }}" data-id="{{$item.ID}}">'
        + '<a> {{ $item.Code }} </a>'
        + '</li>'
        + '{{/items}}</ul>'
    },

    preprocess_receipt_no: {
        'source': Masters.get_preprocess_receipt_no,
        'minLength': 1,
        'template': '<ul class="uk-nav uk-nav-autocomplete uk-autocomplete-results">'
        + '{{~items}}'
        + '<li data-value="{{ $item.Code }}" data-id="{{$item.ID}}">'
        + '<a> {{ $item.Code }} </a>'
        + '</li>'
        + '{{/items}}</ul>'
    },

    preprocess_receipt_item: {
        'source': Masters.get_preprocess_receipt_item_AutoComplete,
        'minLength': 1,
        'template': '<ul class="uk-nav uk-nav-autocomplete uk-autocomplete-results">'
        + ' {{~items}}'
        + '<li data-value="{{ $item.ItemName }}" data-itemid="{{$item.ItemID}}">'
        + '<a> {{ $item.ItemName }}</a>'
        + '</li>'
        + '{{/items}}'
        + '</ul>'
    },

    production_group: {
        'source': Masters.get_production_groups,
        'minLength': 1,
        'template': '<ul class="uk-nav uk-nav-autocomplete uk-autocomplete-results">'
         + '{{~items}}'
         + '<li data-value="{{ $item.Name }}"data-id="{{$item.ID}}"data-std-batch-size="{{$item.StdBatchSize}}"data-Name="{{$item.Name}}">'
         + '<a>{{ $item.Name }}<div>{{ $item.Code }}</div></a>'
         + '</li>'
         + '{{/items}}'
         + '</ul>'
    },

    fso_name: {
        'source': Masters.get_fsoname,
        'minLength': 1,
        'template': '<ul class="uk-nav uk-nav-autocomplete uk-autocomplete-results">'
         + '{{~items}}'
         + '<li data-value="{{ $item.Name }}"data-id="{{$item.EmployeeID}}"data-EmployeeCode="{{$item.Code}}"data-Name="{{$item.Name}}">'
         + '<a>{{ $item.Name }}<div>{{ $item.Code }}</div></a>'
         + '</li>'
         + '{{/items}}'
         + '</ul>'
    },


    doctor: {
        'source': Masters.get_doctor,
        'minLength': 1,
        'template': '<ul class="uk-nav uk-nav-autocomplete uk-autocomplete-results">'
         + '{{~items}}'
         + '<li data-value="{{ $item.Name }}"data-id="{{$item.ID}}">'
         + '<a>{{ $item.Name }}</a>'
         + '</li>'
         + '{{/items}}'
         + '</ul>'
    },

    get_treatment_Item: {
        'source': Masters.get_treatment_medicine,
        'minLength': 1,
        'template': '<ul class="uk-nav uk-nav-autocomplete uk-autocomplete-results">'
         + '{{~items}}'
         + '<li data-value="{{ $item.Name }}"data-id="{{$item.ID}}">'
         + '<a>{{ $item.Name }}</a>'
         + '</li>'
         + '{{/items}}'
         + '</ul>'
    },

    get_treatment_medicineList: {
        'source': Masters.get_treatment_medicineList,
        'minLength': 1,
        'template': '<ul class="uk-nav uk-nav-autocomplete uk-autocomplete-results">'
         + '{{~items}}'
         + '<li data-value="{{ $item.Name }}"data-id="{{$item.ID}}"data-salesunitid="{{$item.SalesUnitID}}"data-salesunit="{{$item.SalesUnit}}"data-primaryunitID="{{$item.PrimaryUnitID}}"data-primaryunit="{{$item.PrimaryUnit}}"data-categoryID="{{$item.CategoryID}}">'
         + '<a>{{ $item.Name }}</a>'
         + '</li>'
         + '{{/items}}'
         + '</ul>'
    },

    get_internal_medicine: {
        'source': Masters.get_internal_medicineList,
        'minLength': 1,
        'template': '<ul class="uk-nav uk-nav-autocomplete uk-autocomplete-results">'
         + '{{~items}}'
         + '<li data-value="{{ $item.Name }}"data-id="{{$item.ID}}"data-salesunitid="{{$item.SalesUnitID}}"data-salesunit="{{$item.SalesUnit}}"data-primaryunitID="{{$item.PrimaryUnitID}}"data-primaryunit="{{$item.PrimaryUnit}}"data-categoryID="{{$item.CategoryID}}">'
         + '<a>{{ $item.Name }}</a>'
         + '</li>'
         + '{{/items}}'
         + '</ul>'
    },

    get_treatments: {
        'source': Masters.get_treatment,
        'minLength': 1,
        'template': '<ul class="uk-nav uk-nav-autocomplete uk-autocomplete-results">'
         + '{{~items}}'
         + '<li data-value="{{ $item.Name }}"data-id="{{$item.ID}}">'
         + '<a>{{ $item.Name }}</a>'
         + '</li>'
         + '{{/items}}'
         + '</ul>'
    },

    patient: {
        'source': Masters.get_patient,
        'minLength': 1,
        'template': '<ul class="uk-nav uk-nav-autocomplete uk-autocomplete-results">'
         + '{{~items}}'
         + '<li data-value="{{ $item.Name }}"data-id="{{$item.ID}}">'
         + '<a>{{ $item.Name }}</a>'
         + '</li>'
         + '{{/items}}'
         + '</ul>'
    },
    get_Appointment_Scheduled: {
        'source': Masters.get_Appointment_Scheduled_patient,
        'minLength': 1,
        'template': '<ul class="uk-nav uk-nav-autocomplete uk-autocomplete-results">'
         + '{{~items}}'
         + '<li data-value="{{ $item.Name }}"data-id="{{$item.ID}}">'
         + '<a>{{ $item.Name }}</a>'
         + '</li>'
         + '{{/items}}'
         + '</ul>'
    },

    get_labitems: {
        'source': Masters.get_lab_test,
        'minLength': 1,
        'template': '<ul class="uk-nav uk-nav-autocomplete uk-autocomplete-results">'
         + '{{~items}}'
         + '<li data-value="{{ $item.Name }}"data-id="{{$item.ID}}"data-category="{{$item.Category}}">'
         + '<a>{{ $item.Name }}</a>'
         + '</li>'
         + '{{/items}}'
         + '</ul>'
    },

    physiotherapy_item: {
        'source': Masters.get_physiotherapy_item,
        'minLength': 1,
        'template': '<ul class="uk-nav uk-nav-autocomplete uk-autocomplete-results">'
         + '{{~items}}'
         + '<li data-value="{{ $item.Name }}"data-id="{{$item.ID}}">'
         + '<a>{{ $item.Name }}</a>'
         + '</li>'
         + '{{/items}}'
         + '</ul>'
    },

    xray_item: {
        'source': Masters.get_xray_item,
        'minLength': 1,
        'template': '<ul class="uk-nav uk-nav-autocomplete uk-autocomplete-results">'
         + '{{~items}}'
         + '<li data-value="{{ $item.Name }}"data-id="{{$item.ID}}">'
         + '<a>{{ $item.Name }}</a>'
         + '</li>'
         + '{{/items}}'
         + '</ul>'
    },
    patient: {
        'source': Masters.get_patient,
        'minLength': 1,
        'template': '<ul class="uk-nav uk-nav-autocomplete uk-autocomplete-results">'
         + '{{~items}}'
         + '<li data-value="{{ $item.Name }}"data-id="{{$item.ID}}">'
         + '<a>{{ $item.Name }}</a>'
         + '</li>'
         + '{{/items}}'
         + '</ul>'
    },
    get_treatment_rooms: {
        'source': Masters.get_treatment_room,
        'minLength': 1,
        'template': '<ul class="uk-nav uk-nav-autocomplete uk-autocomplete-results">'
         + '{{~items}}'
         + '<li data-value="{{ $item.Name }}"data-id="{{$item.ID}}">'
         + '<a>{{ $item.Name }}</a>'
         + '</li>'
         + '{{/items}}'
         + '</ul>'
    },
    get_therapist: {
        'source': Masters.get_therapist_list,
        'minLength': 1,
        'template': '<ul class="uk-nav uk-nav-autocomplete uk-autocomplete-results">'
         + '{{~items}}'
         + '<li data-value="{{ $item.Name }}"data-id="{{$item.ID}}">'
         + '<a>{{ $item.Name }}</a>'
         + '</li>'
         + '{{/items}}'
         + '</ul>'
    },
    get_nursing_station_list: {
        'source': Masters.get_nursing_station,
        'minLength': 1,
        'template': '<ul class="uk-nav uk-nav-autocomplete uk-autocomplete-results">'
         + '{{~items}}'
         + '<li data-value="{{ $item.Name }}"data-id="{{$item.ID}}">'
         + '<a>{{ $item.Name }}</a>'
         + '</li>'
         + '{{/items}}'
         + '</ul>'
    },
    get_warehouse_list: {
        'source': Masters.get_warehouse,
        'minLength': 1,
        'template': '<ul class="uk-nav uk-nav-autocomplete uk-autocomplete-results">'
         + '{{~items}}'
         + '<li data-value="{{ $item.Name }}"data-id="{{$item.ID}}">'
         + '<a>{{ $item.Name }}</a>'
         + '</li>'
         + '{{/items}}'
         + '</ul>'
    },
    get_account_group_list: {
        'source': Masters.get_account_group,
        'minLength': 1,
        'template': '<ul class="uk-nav uk-nav-autocomplete uk-autocomplete-results">'
         + '{{~items}}'
         + '<li data-value="{{ $item.AccountName }}"data-id="{{$item.ID}}">'
         + '<a>{{ $item.AccountName }}</a>'
         + '</li>'
         + '{{/items}}'
         + '</ul>'
    },
    get_account_head_list: {
        'source': Masters.get_account_head,
        'minLength': 1,
        'template': '<ul class="uk-nav uk-nav-autocomplete uk-autocomplete-results">'
         + '{{~items}}'
         + '<li data-value="{{ $item.AccountName }}"data-id="{{$item.ID}}">'
         + '<a>{{ $item.AccountName }}</a>'
         + '</li>'
         + '{{/items}}'
         + '</ul>'

    },

    get_debit_account_list: {
        'source': Masters.get_debit_account_list,
        'minLength': 1,
        'template': '<ul class="uk-nav uk-nav-autocomplete uk-autocomplete-results">'
         + '{{~items}}'
         + '<li data-value="{{ $item.AccountName }}"data-id="{{$item.ID}}">'
         + '<a>{{ $item.AccountName }}</a>'
         + '</li>'
         + '{{/items}}'
         + '</ul>'

    },

    get_credit_account_list: {
        'source': Masters.get_credit_account_list,
        'minLength': 1,
        'template': '<ul class="uk-nav uk-nav-autocomplete uk-autocomplete-results">'
         + '{{~items}}'
         + '<li data-value="{{ $item.AccountName }}"data-id="{{$item.ID}}">'
         + '<a>{{ $item.AccountName }}</a>'
         + '</li>'
         + '{{/items}}'
         + '</ul>'

    },
 

    
}
