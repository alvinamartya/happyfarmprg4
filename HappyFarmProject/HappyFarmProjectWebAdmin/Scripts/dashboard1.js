

$(async function () {
    function formatRupiah(angka, prefix) {
        var minus = angka.toString().substr(0, 1) == '-' ? true : false;

        var number_string = angka.toString().replace(/[^,\d]/g, '').toString(),
            split = number_string.split(','),
            sisa = split[0].length % 3,
            rupiah = split[0].substr(0, sisa),
            ribuan = split[0].substr(sisa).match(/\d{3}/gi);

        // tambahkan titik jika yang di input sudah menjadi angka ribuan
        if (ribuan) {
            separator = sisa ? '.' : '';
            rupiah += separator + ribuan.join('.');
        }

        rupiah = split[1] != undefined ? rupiah + ',' + split[1] : rupiah;

        const result = minus ? "-" + rupiah : rupiah;

        return prefix == undefined ? result : (rupiah ? 'Rp. ' + result : '');
    }

    async function GetData(year, month) {
        const url = 'https://localhost:44301/api/v1/Dashboard/' + month + '/' + year;
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

    async function filledTable(year, month) {
        let dashboardData = await GetData(year, month)
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

        $("#selling-table").empty();
        const n = dashboardData.date.length;
        for (let i = 0; i < n; i++) {
            const table = `
                    <tr>
                        <th scope="row">`+ (i+1) +`</th>
                        <td>`+ dashboardData.date[i] +`</td>
                        <td align="right"> ` + formatRupiah(dashboardData.purchasing[i], "Rp. ") + `</td>
                        <td align="right"> ` + formatRupiah(dashboardData.selling[i], "Rp. ")  + `</td>
                        <td align="right"> ` + formatRupiah((dashboardData.selling[i] - dashboardData.purchasing[i]), "Rp. ") + `</td>
                    </tr>
            `;
            $("#selling-table").append(table);
        }
    }

    async function filledChart(year, month) {
        let dashboardData = await GetData(year, month)
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
                { name: 'Pembelian', data: dashboardData.selling },
                { name: 'Penjualan', data: dashboardData.purchasing },
            ]
        };

        var options = {
            axisX: {
                offset: 30,
                showGrid: false
            },
            axisY: {
                offset: 100,
                labelInterpolationFnc: function (value) {
                    return formatRupiah(value);
                }
            },
            seriesBarDistance: 40,
            chartPadding: {
                top: 15,
                right: 15,
                bottom: 5,
                left: 0
            },
            plugins: [
                Chartist.plugins.tooltip({
                    valueTransform: function (value) {
                        return (value / 1000) + 'k';
                    }
                }),
                Chartist.plugins.legend()
            ],
            width: '100%'
        };

        var responsiveOptions = [
            ['screen and (max-width: 640px)', {
                seriesBarDistance: 2,
                axisX: {
                    labelInterpolationFnc: function (value) {
                        return formatRupiah(value);
                    }
                }
            }]
        ];
        new Chartist.Bar('.net-income', data, options, responsiveOptions);

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
    }

    const date = new Date();
    $("#year").find('option[value="' + date.getFullYear() + '"]').attr('selected', 'selected');
    $("#month").find('option[value="' + (date.getMonth() + 1) + '"]').attr('selected', 'selected');
    filledChart(date.getFullYear(), date.getMonth() + 1);
    filledTable(date.getFullYear(), date.getMonth() + 1);

    $("#year").change(() => {
        filledChart($("#year option:selected").val(), $("#month option:selected").val());
        filledTable($("#year option:selected").val(), $("#month option:selected").val());
    });

    $("#month").change(() => {
        filledChart($("#year option:selected").val(), $("#month option:selected").val());
        filledTable($("#year option:selected").val(), $("#month option:selected").val());
    });
})