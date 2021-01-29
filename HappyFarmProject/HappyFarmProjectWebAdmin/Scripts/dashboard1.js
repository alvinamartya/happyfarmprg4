$(async function () {
    async function GetData() {
        const url = 'https://localhost:44301/api/v1/Dashboard';
        let date = [];
        let selling = [];
        let purchasing = [];

        await $.ajax({
            type: 'GET',
            url: url,
            contentType: "application/json",
            dataType: 'json',
            async: true,
            success: async function (result) {

                result.Data.forEach(e => {
                    const d = new Date(e.Date);
                    const ye = new Intl.DateTimeFormat(['in', 'id'], { year: 'numeric' }).format(d);
                    const mo = new Intl.DateTimeFormat(['in', 'id'], { month: 'short' }).format(d);
                    const da = new Intl.DateTimeFormat(['in', 'id'], { day: '2-digit' }).format(d);

                    date.push(`${da}-${mo}-${ye}`);
                    selling.push(e.TotalSale);
                    purchasing.push(e.TotalPurchase);
                })

            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert('Error: ' + textStatus + ' ' + errorThrown);
            }
        })

        return {
            date: date,
            selling: selling,
            purchasing: purchasing
        };
    }



    let dashboardData = await GetData()
        .then(e => {
            return e;
        })
        .catch(err => {
            return {
                date: [],
                selling: [],
                purchasing: []
            };
        });

    // ============================================================== 
    // income
    // ============================================================== 
    var data = {
        labels: dashboardData.date,
        series: [
            { name: 'Penjualan', data: dashboardData.selling },
            { name: 'Pembelian', data: dashboardData.purchasing },
        ]
    };

    var options = {
        axisX: {
            offset: 30,
            showGrid: false
        },
        axisY: {
            offset: 100
        },
        seriesBarDistance: 40,
        chartPadding: {
            top: 15,
            right: 15,
            bottom: 5,
            left: 0
        },
        plugins: [
            Chartist.plugins.tooltip(),
            Chartist.plugins.legend()
        ],
        width: '100%'
    };

    var responsiveOptions = [
        ['screen and (max-width: 640px)', {
            seriesBarDistance: 2,
            axisX: {
                labelInterpolationFnc: function (value) {
                    return value[0];
                }
            }
        }]
    ];
    new Chartist.Bar('.net-income', data, options, responsiveOptions);

    // Offset x1 a tiny amount so that the straight stroke gets a bounding box
    chart.on('draw', function (ctx) {
        if (ctx.type === 'area') {
            ctx.element.attr({
                x1: ctx.x1 + 0.001
            });
        }

        if (ctx.type === 'label') {
            ctx.element.attr({
                width: '100px' 
            });
        }
    });

    // Create the gradient definition on created event (always after chart re-render)
    chart.on('created', function (ctx) {
        var defs = ctx.svg.elem('defs');
        defs.elem('linearGradient', {
            id: 'gradient',
            x1: 0,
            y1: 1,
            x2: 0,
            y2: 0
        }).elem('stop', {
            offset: 0,
            'stop-color': 'rgba(255, 255, 255, 1)'
        }).parent().elem('stop', {
            offset: 1,
            'stop-color': 'rgba(80, 153, 255, 1)'
        });
    });

    $(window).on('resize', function () {
        chart.update();
    });
})