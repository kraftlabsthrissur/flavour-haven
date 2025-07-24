Employee.get_data= function () {
    var self = Employee;
    var data = {};
    data.ASPNetUserID = clean($("#ASPNetUserID").val());
    data.ID = $("#ID").val();
    data.Code = $("#Code").val();
    data.Title = $("#Title Option:selected").text() != "Select" ? $("#Title Option:selected").val() : '';
    data.Name = $("#Name").val();
    data.DesignationID = $("#DesignationID Option:selected").val();
    data.Gender = $("#Gender Option:selected").text();
    data.MartialStatus = $("#MartialStatus Option:selected").text() != "Select" ? $("#MartialStatus Option:selected").text() : "";
    data.Qualification1 = $("#Qualification1").val();
    data.DOB = $("#DOB").val();
    data.Qualification2 = $("#Qualification2").val();
    data.Qualification3 = $("#Qualification3").val();
    data.BloodGroup = $("#BloodGroupID Option:selected").val() > 0 ? $("#BloodGroupID Option:selected").text() : '';
    data.NoOfDependent = $("#NoOfDependent").val();
    data.NameOfSpouse = $("#NameOfSpouse").val();
    data.NameOfGuardian = $("#NameOfGuardian").val();
    if ($("#IsExcludeFromPayroll").prop('checked') == true) {
        data.IsExcludeFromPayroll = true;
    } else {
        data.IsExcludeFromPayroll = false;
    }
    data.DateOfJoining = $("#DateOfJoining").val();
    data.DateOfConfirmation = $("#DateOfConfirmation").val();
    data.PayGrade = $("#PayGrade").val();
    data.EmpCategoryID = $("#EmpCategoryID Option:selected").val();
    data.PayrollCategoryID = $("#PayrollCategoryID Option:selected").val();
    data.CompanyEmail = $("#CompanyEmail").val();
    data.ReportingCode = $("#ReportingCode").val();
    data.ReportingName = $("#ReportingName").val();
    data.DepartmentID = $("#DepartmentID Option:selected").val();
    data.LocationID = $("#LocationID Option:selected").val();
    data.InterCompanyID = $("#InterCompanyID Option:selected").val();
    data.DateOfSeverance = $("#DateOfSeverance").val();
    data.DateOfReJoining = $("#DateOfReJoining").val();
    data.ProbationDuration = $("#ProbationDuration").val();
    data.EmploymentJobTypeID = $("#EmploymentJobTypeID Option:selected").val();
    data.PrintPayroll = $("#PrintPayroll Option:selected").text() != "Select" ? $("#PrintPayroll Option:selected").text() : '';
    data.PFVoluntaryContribution = $("#PFVoluntaryContribution").val();
    data.PFAccountNo = $("#PFAccountNo").val();
    data.PFUAN = $("#PFUAN").val();
    data.ESINo = $("#ESINo").val();
    data.DoctorFee = $("#DoctorFee").val();
    data.ClinicFee = $("#ClinicFee").val();
    data.IsAlreadyERPUser = $("#IsAlreadyERPUser").val() == "True" ? true : false;
    data.UserName = $("#UserName").val();
    if ($("#PFStatus").prop('checked') == true) {
        data.PFStatus = true;
    } else {
        data.PFStatus = false;
    }
    if ($("#ESIStatus").prop('checked') == true) {
        data.ESIStatus = true;
    } else {
        data.ESIStatus = false;
    }
    if ($("#NPS").prop('checked') == true) {
        data.NPSStatus = true;
    } else {
        data.NPSStatus = false;
    }
    if ($("#MedicalInsurance").prop('checked') == true) {
        data.MedicalInsuranceStatus = true;
    } else {
        data.MedicalInsuranceStatus = false;
    }
    if ($("#AttandancePunching").prop('checked') == true) {
        data.AttandancePunchingStatus = true;
    } else {
        data.AttandancePunchingStatus = false;
    }
    if ($("#MultiLocationPunching").prop('checked') == true) {
        data.MultiLocationPunchingStatus = true;
    } else {
        data.MultiLocationPunchingStatus = false;
    }
    if ($("#SpecialLeave").prop('checked') == true) {
        data.SpecialLeaveStatus = true;
    } else {
        data.SpecialLeaveStatus = false;
    }
    if ($("#Probation").prop('checked') == true) {
        data.ProbationStatus = true;
    } else {
        data.ProbationStatus = false;
    }
    if ($("#ProductionIncentive").prop('checked') == true) {
        data.ProductionIncentiveStatus = true;
    } else {
        data.ProductionIncentiveStatus = false;
    }
    if ($("#SalesIncentive").prop('checked') == true) {
        data.SalesIncentiveStatus = true;
    } else {
        data.SalesIncentiveStatus = false;
    }
    if ($("#FixedIncentive").prop('checked') == true) {
        data.FixedIncentiveStatus = true;
    } else {
        data.FixedIncentiveStatus = false;
    }
    if ($("#IsERPUser").prop('checked') == true) {
        data.IsERPUser = true;
        data.Password = $('#Password').val();
    } else {
        data.IsERPUser = false;
    }
    if ($("#MedicalAid").prop('checked') == true) {
        data.MedicalAidStatus = true;
    } else {
        data.MedicalAidStatus = false;
    }
    if ($("#minimumwages").prop('checked') == true) {
        data.MinimumWagesStatus = true;
    } else {
        data.MinimumWagesStatus = false;
    }
    if ($("#Bonus").prop('checked') == true) {
        data.BonusStatus = true;
    } else {
        data.BonusStatus = false;
    }
    if ($("#professionalTax").prop('checked') == true) {
        data.ProfessionalTaxStatus = true;
    } else {
        data.ProfessionalTaxStatus = false;
    }
    if ($("#WelfareDeduction").prop('checked') == true) {
        data.WelfareDeductionStatus = true;
    } else {
        data.WelfareDeductionStatus = false;
    }
    data.PanNo = $("#PanNo").val();
    data.AadhaarNo = $("#AadhaarNo").val();
    data.AccountNumber = $("#AccountNumber").val();
    data.BankName = $("#BankName").val();
    data.BankBranchName = $("#BankBranchName").val();
    data.IFSC = $("#IFSC").val();
    if ($("#English").prop('checked') == true) {
        data.IsEnglish = true;
    } else {
        data.IsEnglish = false;
    }
    if ($("#Hindi").prop('checked') == true) {
        data.IsHindi = true;
    } else {
        data.IsHindi = false;
    }
    if ($("#Malayalam").prop('checked') == true) {
        data.IsMalayalam = true;
    } else {
        data.IsMalayalam = false;
    }
    if ($("#Tamil").prop('checked') == true) {
        data.IsTamil = true;
    } else {
        data.IsTamil = false;
    }
    if ($("#Telugu").prop('checked') == true) {
        data.IsTelugu = true;
    } else {
        data.IsTelugu = false;
    }
    if ($("#Kannada").prop('checked') == true) {
        data.IsKannada = true;
    } else {
        data.IsKannada = false;
    }

    if ($("#ChangePassword").prop('checked') == true) {
        data.ChangePassword = true;
    } else {
        data.ChangePassword = false;
    }
    data.ExEmployDetails = [];
    var ExEmployDetail = {};
    $('#tbl-ExEmp-address tbody tr ').each(function () {
        ExEmployDetail = {};
        ExEmployDetail.EmployerName = $(this).find(".Employer-name").text();
        ExEmployDetail.StateID = $(this).find(".state-ID").val();
        ExEmployDetail.DistrictID = $(this).find(".district-ID").val();
        ExEmployDetail.CountryID = $(this).find(".country-ID").val();
        ExEmployDetail.Designation = $(this).find(".designation").text();
        ExEmployDetail.ExEmployAddress1 = $(this).find(".address1").val();
        ExEmployDetail.ExEmployAddress2 = $(this).find(".address2").val();
        ExEmployDetail.ExEmployAddress3 = $(this).find(".address3").val();
        ExEmployDetail.ExEmployPlace = $(this).find(".place").val();
        ExEmployDetail.ExEmployPin = $(this).find(".pin").val();
        ExEmployDetail.DateOfJoinning = $(this).find(".date-of-joinning").text();
        ExEmployDetail.DateOfSeverance = $(this).find(".date-of-severance").text();
        ExEmployDetail.ContactPerson = $(this).find(".contact-person").text();
        ExEmployDetail.ContactNumber = $(this).find(".contact-number").text();
        data.ExEmployDetails.push(ExEmployDetail);
    });
    data.AddressList = self.AddressList;
    data.SalaryDetails = [];
    var SalaryDetail = {};
    $('#SalaryComponentList tbody tr ').each(function () {
        SalaryDetail = {};
        if ($(this).find(".salary-components").prop('checked') == true) {
            SalaryDetail.PayType = $(this).find(".paytype").text().trim();
            SalaryDetail.SalaryMonthly = clean($(this).find(".salarymonthly").val());
            SalaryDetail.SalaryAnnual = clean($(this).find(".salaryannual").val());
            if ($(this).find(".finance").prop('checked') == true) {
                SalaryDetail.IsFinancePayRoll = true;
            }
            else {
                SalaryDetail.IsFinancePayRoll = false;
            }
            if ($(this).find(".production").prop('checked') == true) {
                SalaryDetail.IsProductionIncentivePayRoll = true;
            }
            else {
                SalaryDetail.IsProductionIncentivePayRoll = false;
            }
            if ($(this).find(".sales").prop('checked') == true) {
                SalaryDetail.IsSalesIncentivePayRoll = true;
            }
            else {
                SalaryDetail.IsSalesIncentivePayRoll = false;
            }
            data.SalaryDetails.push(SalaryDetail);
        }

    });

    data.FreeMedicineLocationList = self.LocationList

    return data;
},
Employee.save= function () {
    var self = Employee;
    var data = self.get_data();
    var location = "/Masters/Doctor/Index";
    $.ajax({
        url: '/Masters/Doctor/Save',
        data: data,
        dataType: "json",
        type: "POST",
        success: function (response) {
            if (response.Status == "success") {
                app.show_notice("Saved successfully");
                window.location = location;
            } else {
                app.show_error(response.Message);
            }
        }
    });
},
Employee.list= function () {
    $list = $('#employee-list');
    if ($list.length) {
        $list.find('thead.search th').each(function () {
            var title = $(this).text().trim();
            $(this).html('<input type="text" class="md-input" placeholder="' + title + '" />');
        });
        $('#employee-list tbody tr').on('click', function () {
            var Id = $(this).find("td:eq(0) .ID").val();
            window.location = '/Masters/Doctor/Details/' + Id;
        });
        altair_md.inputs();
        var list_table = $list.dataTable({
            "bLengthChange": false,
            "bFilter": true
        });
        list_table.api().columns().each(function (g, h) {
            $('thead.search input').on('keyup change', function () {
                var index = $(this).parent().parent().index();
                list_table.api().column(index).search(this.value).draw();
            });
        });
    }
}