window.oppKanbanSunburst = {
    render: function (element, data) {
        if (!element || !window.ApexCharts) {
            throw new Error('ApexCharts is not available.');
        }

        const chart = new ApexCharts(element, {
            chart: {
                type: 'sunburst',
                height: 420,
                toolbar: { show: false }
            },
            series: [{ data }],
            title: {
                text: 'Opportunities by SA and account owner',
                align: 'left'
            },
            legend: { show: true },
            tooltip: {
                y: {
                    formatter: function (value) {
                        return `${value} opportunit${value === 1 ? 'y' : 'ies'}`;
                    }
                }
            },
            plotOptions: {
                sunburst: {
                    innerSize: '28%',
                    borderRadius: 3,
                    spacing: 2
                }
            }
        });

        return chart.render();
    }
};
