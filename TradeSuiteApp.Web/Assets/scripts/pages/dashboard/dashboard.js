$(function () {
    // dashboard init functions
    dashboard.init();
});
var chart_amt;
var chart_kg;
var chart_values;
var fh_tky;
var fh_pol;
var fh_chu;
var fh_stock_tky;
var fh_stock_pol;
var fh_stock_chu;
var fh_location_wise_sales_in_month;
var fh_location_wise_sales_in_year;
var fh_category_wise_sales_in_month;
var fh_category_wise_sales_in_year;
var fh_production_progress_tky;
var fh_production_progress_pol;
var fh_production_progress_chu;



dashboard = {
    init: function () {
        'use strict';
        var self = dashboard
        self.bind_events();

        if ($("#ct-chart-amt").length) {
            self.chartist_charts_sales_amt_month();
        }
        if ($("#ct-chart-amt-year").length) {
            self.chartist_charts_sales_amt_year();
        }
        if ($("#ct-chart-kg").length) {
            self.chartist_charts_sales_kg_month();
        }

        if ($("#ct-chart-kg-year").length) {
            self.chartist_charts_sales_kg_year();
        }

        if ($("#ct-chart-value").length) {
            self.chartist_charts_sales_value_month();
        }

        if ($("#ct-chart-value-year").length) {
            self.chartist_charts_sales_value_year();
        }

        if ($("#ct-chart-production-month").length) {
            self.chartist_charts_production_kg_month();
        }

        if ($("#ct-chart-production-year").length) {
            self.chartist_charts_production_kg_year();
        }

        if ($("#sales-category-tab").length) {
            self.sales_freeze_headers();
            self.add_category_wise_sales();
        }

        // sales matrix chart
        if ($("#mGraph_sale").length) {
            self.metrics_charts();
        }

        if ($("#stock-category-tab").length) {
            self.stock_freeze_headers();
            self.add_category_wise_stock();
        }

        self.month_wise_sales_in_kg();
        self.month_wise_sales_in_rupees();
        self.month_wise_production();
        self.location_wise_sales_list();
        self.location_wise_sales_freeze_headers();
        self.category_wise_sales_list();
        self.category_wise_sales_freeze_headers();
        self.category_wise_production_progress_list();
        self.production_progress_freeze_headers();

        if ($("#page_content_inner").text().trim() == "") {
            $("#page_content_inner").addClass("common");
            //$("#page_content_inner").html("<img src='Assets/img/_Version1/erp-banner-astra-large.jpg' />");
            $("#page_content_inner").removeClass("dashboard");
        }
    },
    // chartist


    chartist_charts_sales_kg_month: function () {
        var Labels = [];
        var SalesInKgValues = [];
        if ($('#ct-chart-kg').length == 0) {
            return;
        }

        chart_kg = new Chartist.Bar('#ct-chart-kg', {
            labels: Labels,
            series: SalesInKgValues

        },


            {
                // Default mobile configuration
                stackBars: true,
                axisX: {
                    labelInterpolationFnc: function (value) {
                        return value.split(/\s+/).map(function (word) {
                            return word[0];
                        }).join('');
                    }
                },
                axisY: {
                    labelInterpolationFnc: function (value) {
                        return (value / 1) + 'K';
                    },
                    high: 9000,
                    low: 0
                }
            }, [
            // Options override for media > 400px
            ['screen and (min-width: 400px)', {
                reverseData: true,
                horizontalBars: true,
                axisX: {
                    labelInterpolationFnc: Chartist.noop
                },
                axisY: {

                    offset: 60
                }
            }],
            // Options override for media > 800px
            ['screen and (min-width: 800px)', {
                stackBars: false,
                seriesBarDistance: 10
            }],
            // Options override for media > 1000px
            ['screen and (min-width: 1000px)', {
                reverseData: false,
                horizontalBars: false,
                seriesBarDistance: 15
            }]
            ]);

        $.ajax({
            url: '/User/Dashboard/GetMonthWiseSalesSummary?type=chartist_charts_sales_kg_month',
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    $(response.Data.XaxisNames).each(function (i, item) {
                        Labels.push(item.Name);
                    });
                    SalesInKgValues.push([], []);
                    $(response.Data.SalesSummaryValues).each(function (i, item) {
                        SalesInKgValues[0].push(item.BudgetInKgValue)
                        SalesInKgValues[1].push(item.ActualSalesInKgValue)
                    });
                    chart_kg.update();
                } else {
                    console.log(response.Message);
                }
            }
        });
    },

    chartist_charts_sales_amt_month: function () {
        var Labels = [];
        var SalesRevenue = [];
        if ($('#ct-chart-amt').length == 0) {
            return;
        }
        chart_amt = new Chartist.Bar('#ct-chart-amt', {
            labels: Labels,
            series: SalesRevenue
        },
            {
                // Default mobile configuration
                stackBars: true,
                axisX: {
                    labelInterpolationFnc: function (value) {
                        return value.split(/\s+/).map(function (word) {
                            return word[0];
                        }).join('');
                    }
                },
                axisY: {
                    labelInterpolationFnc: function (value) {
                        return (value / 100000) + 'L';
                    },
                    high: 9000000,
                    low: 0
                }
            }, [
            // Options override for media > 400px
            ['screen and (min-width: 400px)', {
                reverseData: true,
                horizontalBars: true,
                axisX: {
                    labelInterpolationFnc: Chartist.noop
                },
                axisY: {
                    offset: 60
                }
            }],
            // Options override for media > 800px
            ['screen and (min-width: 800px)', {
                stackBars: false,
                seriesBarDistance: 10
            }],
            // Options override for media > 1000px
            ['screen and (min-width: 1000px)', {
                reverseData: false,
                horizontalBars: false,
                seriesBarDistance: 15
            }]
            ]);

        $.ajax({
            url: '/User/Dashboard/GetMonthWiseSalesSummary?type=p',
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    $(response.Data.XaxisNames).each(function (i, item) {
                        Labels.push(item.Name);
                    });
                    SalesRevenue.push([], []);
                    $(response.Data.SalesSummaryValues).each(function (i, item) {
                        SalesRevenue[0].push(item.BudgetGrossRevenue)
                        SalesRevenue[1].push(item.ActualSalesRevenue)
                    });
                    chart_amt.update();
                } else {
                    console.log(response.Message);
                }
            }
        });
    },

    chartist_charts_sales_value_month: function () {
        var Labels = [];
        var SalesValues = [];
        if ($('#ct-chart-value').length == 0) {
            return;
        }
        chart_values = new Chartist.Bar('#ct-chart-value', {
            labels: Labels,
            series: SalesValues
        }, {
            // Default mobile configuration
            stackBars: true,
            axisX: {
                labelInterpolationFnc: function (value) {
                    return value.split(/\s+/).map(function (word) {
                        return word[0];
                    }).join('');
                }
            },
            axisY: {
                labelInterpolationFnc: function (value) {
                    return (value / 1000) + 'K';
                },
                high: 9000,
                low: 0
            }
        }, [
            // Options override for media > 400px
            ['screen and (min-width: 400px)', {
                reverseData: true,
                horizontalBars: true,
                axisX: {
                    labelInterpolationFnc: Chartist.noop
                },
                axisY: {
                    offset: 60
                }
            }],
            // Options override for media > 800px
            ['screen and (min-width: 800px)', {
                stackBars: false,
                seriesBarDistance: 10
            }],
            // Options override for media > 1000px
            ['screen and (min-width: 1000px)', {
                reverseData: false,
                horizontalBars: false,
                seriesBarDistance: 15
            }]
        ]);

        $.ajax({
            url: '/User/Dashboard/GetMonthWiseSalesSummary?type=pa',
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    $(response.Data.XaxisNames).each(function (i, item) {
                        Labels.push(item.Name);
                    });
                    SalesValues.push([], []);
                    $(response.Data.SalesSummaryValues).each(function (i, item) {
                        SalesValues[0].push(item.BudgetValue)
                        SalesValues[1].push(item.ActualValue)
                    });
                    chart_values.update();
                } else {
                    console.log(response.Message);
                }
            }
        });
    },

    chartist_charts_production_kg_month: function () {
        var Labels = [];
        var Production = [];
        if ($('#ct-chart-production-month').length == 0) {
            return;
        }
        chart_production_month = new Chartist.Bar('#ct-chart-production-month', {
            labels: Labels,
            series: Production
        }, {
            // Default mobile configuration
            stackBars: true,
            axisX: {
                labelInterpolationFnc: function (value) {
                    return value.split(/\s+/).map(function (word) {
                        return word[0];
                    }).join('');
                }
            },
            axisY: {
                labelInterpolationFnc: function (value) {
                    return (value / 1) + K;
                },
                high: 9000,
                low: 0
            }
        }, [
            // Options override for media > 400px
            ['screen and (min-width: 400px)', {
                reverseData: true,
                horizontalBars: true,
                axisX: {
                    labelInterpolationFnc: Chartist.noop
                },
                axisY: {
                    offset: 60
                }
            }],
            // Options override for media > 800px
            ['screen and (min-width: 800px)', {
                stackBars: false,
                seriesBarDistance: 10
            }],
            // Options override for media > 1000px
            ['screen and (min-width: 1000px)', {
                reverseData: false,
                horizontalBars: false,
                seriesBarDistance: 15
            }]
        ]);

        $.ajax({
            url: '/User/Dashboard/GetTotalProductionInMonth',
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    $(response.Data.XaxisNames).each(function (i, item) {
                        Labels.push(item.Name);
                    });
                    Production.push([]);
                    $(response.Data.TotalProductionQty).each(function (i, item) {
                        Production[0].push(item.TotalProduction)
                    });
                    chart_production_month.update();
                } else {
                    console.log(response.Message);
                }
            }
        });
    },

    chartist_charts_sales_kg_year: function () {
        var Labels = [];
        var SalesInKgValues = [];
        if ($('#ct-chart-kg-year').length == 0) {
            return;
        }

        chart_kg_year = new Chartist.Bar('#ct-chart-kg-year', {
            labels: Labels,
            series: SalesInKgValues

        }, {
            // Default mobile configuration
            stackBars: true,
            axisX: {
                labelInterpolationFnc: function (value) {
                    return value.split(/\s+/).map(function (word) {
                        return word[0];
                    }).join('');
                }
            },
            axisY: {
                labelInterpolationFnc: function (value) {
                    return (value / 1000) + 'K';
                },
                high: 9000,
                low: 0
            }
        }, [
            // Options override for media > 400px
            ['screen and (min-width: 400px)', {
                reverseData: true,
                horizontalBars: true,
                axisX: {
                    labelInterpolationFnc: Chartist.noop
                },
                axisY: {
                    offset: 60
                }
            }],
            // Options override for media > 800px
            ['screen and (min-width: 800px)', {
                stackBars: false,
                seriesBarDistance: 10
            }],
            // Options override for media > 1000px
            ['screen and (min-width: 1000px)', {
                reverseData: false,
                horizontalBars: false,
                seriesBarDistance: 15
            }]
        ]);

        $.ajax({
            url: '/User/Dashboard/GetYearWiseSalesSummary?type=chart_kg_year',
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    $(response.Data.XaxisNames).each(function (i, item) {
                        Labels.push(item.Name);
                    });
                    SalesInKgValues.push([], []);
                    $(response.Data.SalesSummaryValues).each(function (i, item) {
                        SalesInKgValues[0].push(item.BudgetInKgValue)
                        SalesInKgValues[1].push(item.ActualSalesInKgValue)
                    });
                    chart_kg_year.update();
                } else {
                    console.log(response.Message);
                }
            }
        });
    },

    chartist_charts_sales_amt_year: function () {
        var Labels = [];
        var SalesRevenue = [];
        if ($('#ct-chart-amt-year').length == 0) {
            return;
        }
        chart_amt_year = new Chartist.Bar('#ct-chart-amt-year', {
            labels: Labels,
            series: SalesRevenue
        }, {
            // Default mobile configuration
            stackBars: true,
            axisX: {
                labelInterpolationFnc: function (value) {
                    return value.split(/\s+/).map(function (word) {
                        return word[0];
                    }).join('');
                }
            },
            axisY: {
                labelInterpolationFnc: function (value) {
                    return (value / 100000) + 'L';
                },
                high: 90000000,
                low: 0
            }
        }, [
            // Options override for media > 400px
            ['screen and (min-width: 400px)', {
                reverseData: true,
                horizontalBars: true,
                axisX: {
                    labelInterpolationFnc: Chartist.noop
                },
                axisY: {
                    offset: 60
                }
            }],
            // Options override for media > 800px
            ['screen and (min-width: 800px)', {
                stackBars: false,
                seriesBarDistance: 10
            }],
            // Options override for media > 1000px
            ['screen and (min-width: 1000px)', {
                reverseData: false,
                horizontalBars: false,
                seriesBarDistance: 15
            }]
        ]);

        $.ajax({
            url: '/User/Dashboard/GetYearWiseSalesSummary?type=chart_amt_year',
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    $(response.Data.XaxisNames).each(function (i, item) {
                        Labels.push(item.Name);
                    });
                    SalesRevenue.push([], []);
                    $(response.Data.SalesSummaryValues).each(function (i, item) {
                        SalesRevenue[0].push(item.BudgetGrossRevenue)
                        SalesRevenue[1].push(item.ActualSalesRevenue)
                    });
                    chart_amt_year.update();
                } else {
                    console.log(response.Message);
                }
            }
        });
    },

    chartist_charts_sales_value_year: function () {
        var Labels = [];
        var SalesValues = [];
        if ($('#ct-chart-value-year').length == 0) {
            return;
        }
        chart_values_year = new Chartist.Bar('#ct-chart-value-year', {
            labels: Labels,
            series: SalesValues
        }, {
            // Default mobile configuration
            stackBars: true,
            axisX: {
                labelInterpolationFnc: function (value) {
                    return value.split(/\s+/).map(function (word) {
                        return word[0];
                    }).join('');
                }
            },
            axisY: {
                labelInterpolationFnc: function (value) {
                    return (value / 1000) + 'K';
                },
                high: 9000,
                low: 0
            }
        }, [
            // Options override for media > 400px
            ['screen and (min-width: 400px)', {
                reverseData: true,
                horizontalBars: true,
                axisX: {
                    labelInterpolationFnc: Chartist.noop
                },
                axisY: {
                    offset: 60
                }
            }],
            // Options override for media > 800px
            ['screen and (min-width: 800px)', {
                stackBars: false,
                seriesBarDistance: 10
            }],
            // Options override for media > 1000px
            ['screen and (min-width: 1000px)', {
                reverseData: false,
                horizontalBars: false,
                seriesBarDistance: 15
            }]
        ]);

        $.ajax({
            url: '/User/Dashboard/GetYearWiseSalesSummary?type=chart_values_year',
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    $(response.Data.XaxisNames).each(function (i, item) {
                        Labels.push(item.Name);
                    });

                    SalesValues.push([], []);
                    $(response.Data.SalesSummaryValues).each(function (i, item) {
                        SalesValues[0].push(item.BudgetValue)
                        SalesValues[1].push(item.ActualValue)

                    });
                    chart_values_year.update();
                } else {
                    console.log(response.Message);
                }
            }
        });
    },

    chartist_charts_production_kg_year: function () {
        var Labels = [];
        var Production = [];
        if ($('#ct-chart-production-year').length == 0) {
            return;
        }
        chart_production_year = new Chartist.Bar('#ct-chart-production-year', {
            labels: Labels,
            series: Production
        }, {
            // Default mobile configuration
            stackBars: true,
            axisX: {
                labelInterpolationFnc: function (value) {
                    return value.split(/\s+/).map(function (word) {
                        return word[0];
                    }).join('');
                }
            },
            axisY: {
                labelInterpolationFnc: function (value) {
                    return (value / 1) + 'K';
                },
                high: 9000,
                low: 0
            }
        }, [
            // Options override for media > 400px
            ['screen and (min-width: 400px)', {
                reverseData: true,
                horizontalBars: true,
                axisX: {
                    labelInterpolationFnc: Chartist.noop
                },
                axisY: {
                    offset: 60
                }
            }],
            // Options override for media > 800px
            ['screen and (min-width: 800px)', {
                stackBars: false,
                seriesBarDistance: 10
            }],
            // Options override for media > 1000px
            ['screen and (min-width: 1000px)', {
                reverseData: false,
                horizontalBars: false,
                seriesBarDistance: 15
            }]
        ]);

        $.ajax({
            url: '/User/Dashboard/GetTotalProductionInYear',
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    $(response.Data.XaxisNames).each(function (i, item) {
                        Labels.push(item.Name);
                    });

                    Production.push([]);
                    $(response.Data.TotalProductionQty).each(function (i, item) {
                        Production[0].push(item.TotalProduction)

                    });
                    chart_production_year.update();
                } else {
                    console.log(response.Message);
                }
            }
        });
    },

    month_wise_production: function () {
        var Labels = [];
        var Production = [];
        var High = 100000;
        if ($('#ct-chart-month-wise-production').length == 0) {
            return;
        }
        month_wise_production = new Chartist.Bar('#ct-chart-month-wise-production', {
            labels: Labels,
            series: Production
        },
 {
     // Default mobile configuration
     plugins: [
                     Chartist.plugins.tooltip({
                         currency: ' ',
                     })
     ],

     stackBars: true,
     axisX: {
         labelInterpolationFnc: function (value) {
             return value.split(/\s+/).map(function (word) {
                 return word[0];
             }).join('');
         }
     },
     axisY: {
         labelInterpolationFnc: function (value) {
             return (value / 1000) + 'K';
         },
         high: High,
         low: 0
     }
 },

            [
            // Options override for media > 400px
            ['screen and (min-width: 400px)', {
                reverseData: true,
                horizontalBars: true,
                axisX: {
                    labelInterpolationFnc: Chartist.noop
                },
                axisY: {
                    offset: 60
                }
            }],
            // Options override for media > 800px
            ['screen and (min-width: 800px)', {
                stackBars: false,
            }],
            // Options override for media > 1000px
            ['screen and (min-width: 1000px)', {
                reverseData: false,
                horizontalBars: false

            }]
            ]);

        $.ajax({
            url: '/User/Dashboard/GetMonthWiseProduction?type=month_wise_production',
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    $(response.Data.XaxisNames).each(function (i, item) {
                        Labels.push(item.Name);
                    });
                    Production.push([], [], []);
                    $(response.Data.MonthWiseProductionQty).each(function (i, item) {
                        Production[0].push(item.TotalProductionInTKY);
                        Production[1].push(item.TotalProductionInPOL);
                        Production[2].push(item.TotalProductionInCHU);
                    });

                    month_wise_production.update();
                } else {

                    console.log(response.Message);
                }
            }
        });
    },

    month_wise_sales_in_kg: function () {
        var self = dashboard;
        $('#month-wise-sales-in-kg-tab').on('change.uk.tab', function (event, active_item, previous_item) {
            self.tabbed_month_wise_sales_in_kg(active_item.data('tab'));
        });
    },

    tabbed_month_wise_sales_in_kg: function (type) {
        var self = dashboard;
        var Labels = [];
        var SalesInKgValues = [];
        var $chart;
        switch (type) {
            case "VOS TKY":
                $chart = '#ct-chart-kg-tky';
                break;
            case "VOS POL":
                $chart = '#ct-chart-kg-pol';
                break;
            case "VOS CHU":
                $chart = '#ct-chart-kg-chu';
                break;
            default:
                $chart = '#ct-chart-kg-tky';
        }
        if ($($chart).length == 0) {
            return;
        }
        month_wise_sales_in_kg = new Chartist.Bar($chart, {
            labels: Labels,
            series: SalesInKgValues
        },
            {
                // Default mobile configuration
                plugins: [
                     Chartist.plugins.tooltip({
                         currency: ' ',
                     })
                ],

                stackBars: true,
                axisX: {
                    labelInterpolationFnc: function (value) {
                        return value.split(/\s+/).map(function (word) {
                            return word[0];
                        }).join('');
                    }
                },
                axisY: {
                    labelInterpolationFnc: function (value) {
                        return (value / 1000) + 'K';
                    },
                    high: 250000,
                    low: 0
                }
            },

            [
            // Options override for media > 400px
            ['screen and (min-width: 400px)', {
                reverseData: true,
                horizontalBars: true,
                axisX: {
                    labelInterpolationFnc: Chartist.noop
                },
                axisY: {
                    offset: 60
                }
            }],
            // Options override for media > 800px
            ['screen and (min-width: 800px)', {
                reverseData: false,
                stackBars: false,
                seriesBarDistance: 10
            }],
            // Options override for media > 1000px
            ['screen and (min-width: 1000px)', {
                reverseData: false,
                horizontalBars: false,
                // seriesBarDistance: 15
            }]
            ]);

        $.ajax({
            url: '/User/Dashboard/GetMonthWiseSalesByLocationHead?type=month_wise_sales_in_kg_tky',
            data: {
                LocationHead: type,
                Batchtype: $("#sales-in-kg-batchtype").val(),
            },
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    $(response.Data.XaxisNames).each(function (i, item) {
                        Labels.push(item.Name);
                    });
                    SalesInKgValues.push([], [], []);
                    $(response.Data.SalesSummaryValues).each(function (i, item) {
                        SalesInKgValues[0].push(item.PrevoiusYearSalesInKgValue)
                        SalesInKgValues[1].push(item.BudgetInKgValue)
                        SalesInKgValues[2].push(item.ActualSalesInKgValue)

                    });
                    month_wise_sales_in_kg.update();
                } else {
                    console.log(response.Message);
                }
            }
        });
    },

    month_wise_sales_in_rupees: function () {
        var self = dashboard;
        $('#month-wise-sales-in-rs-tab').on('change.uk.tab', function (event, active_item, previous_item) {
            self.tabbed_month_wise_sales_in_rupees(active_item.data('tab'));
        });
    },

    tabbed_month_wise_sales_in_rupees: function (type) {
        var self = dashboard;
        var Labels = [];
        var SalesRevenue = [];
        var $chart;
        switch (type) {
            case "VOS TKY":
                $chart = '#ct-chart-rs-tky';
                break;
            case "VOS POL":
                $chart = '#ct-chart-rs-pol';
                break;
            case "VOS CHU":
                $chart = '#ct-chart-rs-chu';
                break;
            default:
                $chart = '#ct-chart-rs-tky';
        }
        if ($($chart).length == 0) {
            return;
        }

        month_wise_sales_in_rupees = new Chartist.Bar($chart, {
            labels: Labels,
            series: SalesRevenue
        },

            {
                // Default mobile configuration
                plugins: [
                    Chartist.plugins.tooltip({
                        currency: ' ',
                    })
                ],
                stackBars: true,
                axisX: {
                    labelInterpolationFnc: function (value) {
                        return value.split(/\s+/).map(function (word) {
                            return word[0];
                        }).join('');
                    }
                },
                axisY: {
                    labelInterpolationFnc: function (value) {
                        return (value / 100000) + 'L';
                    },
                    high: 150000000,
                    low: 0
                }
            }, [
            // Options override for media > 400px
            ['screen and (min-width: 400px)', {
                reverseData: true,
                horizontalBars: true,
                axisX: {
                    labelInterpolationFnc: Chartist.noop
                },
                axisY: {
                    offset: 60
                }
            }],
            // Options override for media > 800px
            ['screen and (min-width: 800px)', {
                stackBars: false,
                seriesBarDistance: 10
            }],
            // Options override for media > 1000px
            ['screen and (min-width: 1000px)', {
                reverseData: false,
                horizontalBars: false,
            }]
            ]);
        $.ajax({
            url: '/User/Dashboard/GetMonthWiseSalesByLocationHead?type=month_wise_sales_in_rupees_tky',
            data: {
                LocationHead: type,
                Batchtype: $("#sales-in-rupees-batchtype").val(),
            },
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    $(response.Data.XaxisNames).each(function (i, item) {
                        Labels.push(item.Name);
                    });
                    SalesRevenue.push([], [], []);
                    $(response.Data.SalesSummaryValues).each(function (i, item) {
                        SalesRevenue[0].push(item.PrevoiusYearGrossRevenue)
                        SalesRevenue[1].push(item.BudgetGrossRevenue)
                        SalesRevenue[2].push(item.ActualSalesRevenue)

                    });
                    month_wise_sales_in_rupees.update();
                } else {
                    console.log(response.Message);
                }
            }
        });
    },

    add_category_wise_sales: function () {
        var self = dashboard;
        $.ajax({
            url: '/User/Dashboard/GetCategoryWiseSalesWithLocationHead',
            dataType: "json",
            type: "POST",
            success: function (response) {
                var content_tky = "";
                var $content_tky;
                var content_pol = "";
                var $content_pol;
                var content_chu = "";
                var $content_chu;

                $(response.Data).each(function (i, item) {
                    if (item.LocationHead == "VOS TKY") {
                        content_tky += '<tr>'
                            + '<td>' + item.Category + '</td>'
                            + '<td class="mask-numeric">' + item.ActualSalesInKgValue + '</td>'
                            + '<td class="mask-currency">' + item.ActualSalesRevenue + '</td>'
                            + '<td class="mask-numeric">' + item.BudgetInKgValue + '</td>'
                            + '<td class="mask-currency">' + item.BudgetGrossRevenue + '</td>'
                            + '</tr>';
                    }
                    else if (item.LocationHead == "VOS POL") {
                        content_pol += '<tr>'
                            + '<td>' + item.Category + '</td>'
                            + '<td class="mask-numeric">' + item.ActualSalesInKgValue + '</td>'
                            + '<td class="mask-currency">' + item.ActualSalesRevenue + '</td>'
                            + '<td class="mask-numeric">' + item.BudgetInKgValue + '</td>'
                            + '<td class="mask-currency">' + item.BudgetGrossRevenue + '</td>'
                            + '</tr>';
                    }
                    else {
                        content_chu += '<tr>'
                           + '<td>' + item.Category + '</td>'
                           + '<td class="mask-numeric">' + item.ActualSalesInKgValue + '</td>'
                           + '<td class="mask-currency">' + item.ActualSalesRevenue + '</td>'
                           + '<td class="mask-numeric">' + item.BudgetInKgValue + '</td>'
                           + '<td class="mask-currency">' + item.BudgetGrossRevenue + '</td>'
                           + '</tr>';
                    }
                });
                $content_tky = $(content_tky);
                app.format($content_tky);
                $('#Category-Wise-Sales_Table-Tky tbody').append($content_tky);

                $content_pol = $(content_pol);
                app.format($content_pol);
                $('#Category-Wise-Sales_Table-Pol tbody').append($content_pol);

                $content_chu = $(content_chu);
                app.format($content_chu);
                $('#Category-Wise-Sales_Table-Chu tbody').append($content_chu);



            }
        });
    },

    add_category_wise_stock: function () {
        var self = dashboard;
        $.ajax({
            url: '/User/Dashboard/GetBusinessCategoryWiseStock',
            dataType: "json",
            type: "POST",
            success: function (response) {
                var content_tky = "";
                var $content_tky;
                var content_pol = "";
                var $content_pol;
                var content_chu = "";
                var $content_chu;
                $(response.Data).each(function (i, item) {
                    if (item.LocationHead == "VOS TKY") {
                        content_tky += '<tr>'
                            + '<td>' + item.BusinessCategory + '</td>'
                            + '<td class="mask-currency">' + item.Stock + '</td>'
                            + '</tr>';
                    }
                    else if (item.LocationHead == "VOS POL") {
                        content_pol += '<tr>'
                            + '<td>' + item.BusinessCategory + '</td>'
                            + '<td class="mask-currency">' + item.Stock + '</td>'
                            + '</tr>';
                    }
                    else {
                        content_chu += '<tr>'
                            + '<td>' + item.BusinessCategory + '</td>'
                            + '<td class="mask-currency">' + item.Stock + '</td>'
                            + '</tr>';
                    }
                });
                $content_tky = $(content_tky);
                app.format($content_tky);
                $('#category-wise-stock_table-tky tbody').append($content_tky);

                $content_pol = $(content_pol);
                app.format($content_pol);
                $('#category-wise-stock_table-pol tbody').append($content_pol);

                $content_chu = $(content_chu);
                app.format($content_chu);
                $('#category-wise-stock_table-chu tbody').append($content_chu);


            }
        });
    },

    location_wise_sales_list: function () {
        var self = dashboard;
        $('#location-wise-sales-tab').on('change.uk.tab', function (event, active_item, previous_item) {
            self.tabbed_location_wise_sales_list(active_item.data('tab'));
        });
    },

    tabbed_location_wise_sales_list: function (type) {
        var self = dashboard;
        var content = "";
        var freeze_header
        var batchtype = $("#location-wise-sales-batchtype").val();
        var $content;
        var $list;
        switch (type) {
            case "year-wise":
                $list = $('#location-wise-sales-year-table');
                $('#location-wise-sales-year-table tbody tr').remove();
                break;
            default:
                $list = $('#location-wise-sales-month-table');
                $('#location-wise-sales-month-table tbody tr').remove();
        }

        $.ajax({
            url: '/User/Dashboard/GetLocationWiseSales?type=' + type,
            data: {
                Type: type,
                Batchtype: batchtype
            },
            dataType: "json",
            type: "POST",
            success: function (response) {
                $(response.Data).each(function (i, item) {
                    var slno = (i + 1);
                    content += '<tr>'
                            + '<td class="uk-text-center">' + slno + '</td>'
                            + '<td>' + item.Location + '</td>'
                            + '<td class="mask-numeric">' + item.BudgetInKgValue + '</td>'
                            + '<td class="mask-numeric">' + item.ActualSalesInKgValue + '</td>'
                            + '<td class="mask-currency">' + item.BudgetGrossRevenue + '</td>'
                            + '<td class="mask-currency">' + item.ActualSalesRevenue + '</td>'
                            + '</tr>';


                });
                $content = $(content);
                app.format($content);
                $list.append($content);

            }
        });
    },

    category_wise_sales_list: function () {
        var self = dashboard;
        $('#category-wise-sales-tab').on('change.uk.tab', function (event, active_item, previous_item) {
            self.tabbed_category_wise_sales_list(active_item.data('tab'));
        });
    },

    tabbed_category_wise_sales_list: function (type) {
        var self = dashboard;
        var freeze_header
        var batchtype = $("#category-wise-sales-batchtype").val();
        var content = "";
        var $content;
        var $list;
        switch (type) {
            case "year-wise":
                $list = $('#category-wise-sales-year-table');
                $('#category-wise-sales-year-table tbody tr').remove();
                break;
            default:
                $list = $('#category-wise-sales-month-table');
                $('#category-wise-sales-month-table tbody tr').remove();
        }

        $.ajax({
            url: '/User/Dashboard/GetCategoryWiseSales?type=' + type,
            data: {
                Type: type,
                Batchtype: batchtype
            },
            dataType: "json",
            type: "POST",
            success: function (response) {
                $(response.Data).each(function (i, item) {
                    var slno = (i + 1);
                    content += '<tr>'
                        + '<td class="uk-text-center">' + slno + '</td>'
                        + '<td>' + item.Category + '</td>'
                        + '<td class="mask-numeric">' + item.BudgetInKgValue + '</td>'
                        + '<td class="mask-numeric">' + item.ActualSalesInKgValue + '</td>'
                        + '<td class="mask-currency">' + item.BudgetGrossRevenue + '</td>'
                        + '<td class="mask-currency">' + item.ActualSalesRevenue + '</td>'
                        + '</tr>';
                });
                $content = $(content);
                app.format($content);
                $list.append($content);
            }
        });
    },

    category_wise_production_progress_list: function () {
        var self = dashboard;
        $('#production-in-progress-tab').on('change.uk.tab', function (event, active_item, previous_item) {
            self.tabbed_category_wise_production_progress_list(active_item.data('tab'));
        });
    },

    tabbed_category_wise_production_progress_list: function (type) {
        var self = dashboard;
        var category_id = $("#production-progress-category-id").val();
        var content = "";
        var $content;
        var $list;
        switch (type) {
            case "VOS POL":
                $list = $('#production-in-progress_table-pol');
                $('#production-in-progress_table-pol tbody tr').remove();
                break;
            case "VOS CHU":
                $list = $('#production-in-progress_table-chu');
                $('#production-in-progress_table-chu tbody tr').remove();
                break;
            case "VOS TKY":
                $list = $('#production-in-progress_table-tky');
                $('#production-in-progress_table-tky tbody tr').remove();
                break;
        }

        $.ajax({
            url: '/User/Dashboard/GetProductionQtyInProgress?type=' + type,
            data: {
                Type: type,
                SalesCategoryID: category_id
            },
            dataType: "json",
            type: "POST",
            success: function (response) {
                $(response.Data).each(function (i, item) {
                    var slno = (i + 1);
                    if (item.ExpectedDate == "01-01-1900") {
                        item.ExpectedDate = ""
                    }
                    content += '<tr>'
                        + '<td class="uk-text-center">' + slno + '</td>'
                        + '<td>' + item.BatchNo + '</td>'
                        + '<td>' + item.ItemName + '</td>'
                        + '<td class="mask-numeric">' + item.StandardOutput + '</td>'
                        + '<td>' + item.ExpectedDate + '</td>'
                        + '</tr>';
                });
                $content = $(content);
                app.format($content);
                $list.append($content);

            }
        });
    },

    bind_events: function () {
        var self = dashboard;
        $("body").on('click', '.sales-kg-month-tab', self.chartist_charts_sales_kg_month);
        $("body").on('click', '.sales-kg-year-tab', self.chartist_charts_sales_kg_year);
        $("body").on('click', '.sales-rupees-month-tab', self.chartist_charts_sales_amt_month);
        $("body").on('click', '.sales-rupees-year-tab', self.chartist_charts_sales_amt_year);
        $("body").on('click', '.sales-value-month-tab', self.chartist_charts_sales_value_month);
        $("body").on('click', '.sales-value-year-tab', self.chartist_charts_sales_value_year);
        $("body").on('click', '.production-month-tab', self.chartist_charts_production_kg_month);
        $("body").on('click', '.production-year-tab', self.chartist_charts_production_kg_year);


        $("body").on("change", "#production-progress-category-id", self.on_change_production_progress_category);
        $("body").on("change", "#category-wise-sales-batchtype", self.on_change_category_wise_sales_batchtype);
        $("body").on("change", "#location-wise-sales-batchtype", self.on_change_location_wise_sales_batchtype);
        $("body").on("change", "#sales-in-rupees-batchtype", self.on_change_sales_in_rupees_batchtype);
        $("body").on("change", "#sales-in-kg-batchtype", self.on_change_sales_in_kg_batchtype);
    },

    on_change_sales_in_kg_batchtype: function () {
        var self = dashboard;
        var sales_in_kg_current_tab = $('ul#month-wise-sales-in-kg-tab li.uk-active').not(".uk-hidden").data("tab");
        self.tabbed_month_wise_sales_in_kg(sales_in_kg_current_tab)
    },

    on_change_sales_in_rupees_batchtype: function () {
        var self = dashboard;
        var sales_in_rupees_current_tab = $('ul#month-wise-sales-in-rs-tab li.uk-active').not(".uk-hidden").data("tab");
        self.tabbed_month_wise_sales_in_rupees(sales_in_rupees_current_tab)
    },

    on_change_location_wise_sales_batchtype: function () {
        var self = dashboard;
        var location_wise_sales_current_tab = $('ul#location-wise-sales-tab li.uk-active').not(".uk-hidden").data("tab");
        self.tabbed_location_wise_sales_list(location_wise_sales_current_tab)
    },

    on_change_category_wise_sales_batchtype: function () {
        var self = dashboard;
        var category_wise_sales_current_tab = $('ul#category-wise-sales-tab li.uk-active').not(".uk-hidden").data("tab");
        self.tabbed_category_wise_sales_list(category_wise_sales_current_tab)
    },

    on_change_production_progress_category: function () {
        var self = dashboard;
        var production_progress_current_tab = $('ul#production-in-progress-tab li.uk-active').not(".uk-hidden").data("tab");
        self.tabbed_category_wise_production_progress_list(production_progress_current_tab)
    },

    sales_freeze_headers: function () {
        fh_tky = $("#Category-Wise-Sales_Table-Tky").FreezeHeader({ height: 250 });
        fh_pol = $("#Category-Wise-Sales_Table-Pol").FreezeHeader({ height: 250 });
        fh_chu = $("#Category-Wise-Sales_Table-Chu").FreezeHeader({ height: 250 });
        $('#sales-category-tab[data-uk-tab]').on('change.uk.tab', function (event, active_item, previous_item) {
            setTimeout(function () {
                if ($(active_item).attr('id') == "tky-sales-tab") {
                    fh_tky.resizeHeader();
                } else if ($(active_item).attr('id') == "pol-sales-tab") {
                    fh_pol.resizeHeader();
                }
                else {
                    fh_chu.resizeHeader();
                }
            }, 500);
        });
    },

    location_wise_sales_freeze_headers: function () {
        fh_location_wise_sales_in_month = $("#location-wise-sales-month-table").FreezeHeader({ height: 250 });
        fh_location_wise_sales_in_year = $("#location-wise-sales-year-table").FreezeHeader({ height: 250 });
        $('#location-wise-sales-tab[data-uk-tab]').on('change.uk.tab', function (event, active_item, previous_item) {
            setTimeout(function () {
                if ($(active_item).attr('id') == "location-wise-sales-month-tab") {
                    fh_location_wise_sales_in_month.resizeHeader();
                } else {
                    fh_location_wise_sales_in_year.resizeHeader();
                }

            }, 500);
        });
    },

    category_wise_sales_freeze_headers: function () {
        fh_category_wise_sales_in_month = $("#category-wise-sales-month-table").FreezeHeader({ height: 250 });
        fh_category_wise_sales_in_year = $("#category-wise-sales-year-table").FreezeHeader({ height: 250 });
        $('#category-wise-sales-tab[data-uk-tab]').on('change.uk.tab', function (event, active_item, previous_item) {
            setTimeout(function () {
                if ($(active_item).attr('id') == "category-wise-sales-month-tab") {
                    fh_category_wise_sales_in_month.resizeHeader();
                } else {
                    fh_category_wise_sales_in_year.resizeHeader();
                }

            }, 500);
        });
    },

    production_progress_freeze_headers: function () {
        fh_production_progress_tky = $("#production-in-progress_table-tky").FreezeHeader({ height: 250 });
        fh_production_progress_pol = $("#production-in-progress_table-pol").FreezeHeader({ height: 250 });
        fh_production_progress_chu = $("#production-in-progress_table-chu").FreezeHeader({ height: 250 });
        $('#production-in-progress-tab[data-uk-tab]').on('change.uk.tab', function (event, active_item, previous_item) {
            setTimeout(function () {
                if ($(active_item).attr('id') == "tky-production-progress-tab") {
                    fh_production_progress_tky.resizeHeader();
                }
                else if ($(active_item).attr('id') == "pol-production-progress-tab") {
                    fh_production_progress_pol.resizeHeader();
                }
                else {
                    fh_production_progress_chu.resizeHeader();
                }

            }, 800);
        });
    },

    stock_freeze_headers: function () {
        fh_stock_tky = $("#category-wise-stock_table-tky").FreezeHeader({ height: 250 });
        fh_stock_pol = $("#category-wise-stock_table-pol").FreezeHeader({ height: 250 });
        fh_stock_chu = $("#category-wise-stock_table-chu").FreezeHeader({ height: 250 });

        $('#stock-category-tab[data-uk-tab]').on('change.uk.tab', function (event, active_item, previous_item) {
            setTimeout(function () {
                if ($(active_item).attr('id') == "tky-stock-tab") {
                    fh_stock_tky.resizeHeader();
                } else if ($(active_item).attr('id') == "pol-stock-tab") {
                    fh_stock_pol.resizeHeader();
                }
                else {
                    fh_stock_chu.resizeHeader();
                }
            }, 500);
        });
    },

    // metrics-graphics
    metrics_charts: function () {

        var self = dashboard;

        self.buildGraph_sale();
        $window.on('resize', function () {
            self.buildGraph_sale();
        });
        $("#mGraph_sale").on('display.uk.check', function () {
            self.buildGraph_sale();
        });

    },

    buildGraph_sale: function () {
        var mGraph_sale = '#mGraph_sale';
        var data = [];
        var items = {};

        var $thisEl_height = 0;

        if ($thisEl_height == 0) {
            var $thisEl_height = 240;
        }
        var $thisEl_height = 340;
        var $thisEl_width = $(mGraph_sale).width();

        $.ajax({
            url: '/User/Dashboard/GetSalesAmountByDayWise',
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.Status == "success") {
                    data = response.Data;
                    var d;
                    for (var i = 0; i < data.length; i++) {
                        d = [data[i]];
                        data[i] = MG.convert.date(d, 'DateString', '%Y-%m-%d')[0];
                    }
                    var markers = [
                        {
                            'date': new Date('2016-02-26T00:00:00.000Z'),
                            'label': 'Winter Sale'
                        },
                        {
                            'date': new Date('2016-06-02T00:00:00.000Z'),
                            'label': 'Spring Sale'
                        }
                    ];

                    // add a chart that has a log scale
                    MG.data_graphic({
                        data: data,
                        y_scale_type: 'log',
                        area: true,
                        markers: 0,
                        linked: false,
                        width: $thisEl_width,
                        height: $thisEl_height,
                        right: 20,
                        target: mGraph_sale,
                        interpolate_tension: 0.7,
                        chart_type: 'line',
                        // markers: markers,
                        x_accessor: 'DateString',
                        y_accessor: 'Value',
                        flip_area_under_y_value: -1000,
                    });
                }
                else {
                    console.log(response.Message);
                }
            }
        });
    }

}
