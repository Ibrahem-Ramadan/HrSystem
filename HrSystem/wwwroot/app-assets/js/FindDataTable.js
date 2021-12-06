var dTable;
/* Initialize Datatables */
function FindDataTable(LoadUrl, tableid, IsDataSearch, _pageLength, _SearchObj) {
    //  تعريب الداتا تيبل
    
    // يمكن  الغاء  الكومنت الاسفل حال العمل باللغتين العربيه  والانجليزية
    // if ($('html').prop("lang") == "ar") { dt_lan = arabic; }
    var SearchBH = 'Search'
    
    // if ($('html').prop("lang") == "ar") { SearchBH = 'ايحث'; }
    var tools = $("th:not(.tools-th ,.check-head)");
    /* Initialize Datatables */
    dTable = $("#" + tableid).DataTable({
        "ajax": {
            "url": LoadUrl,
            "type": "POST",
            "data": { SearchObj: _SearchObj }
        },
        order: [[0, "desc"]],
        "buttons": [
            {
                'extend': 'collection',
                'text': 'Export',
                'buttons': [
                    'copy',
                    'excel',
                    'csv',
                    'pdf',
                    'print'
                ]
            }
        ],
        "columns": DataTableColumns,
        //"columnDefs": [
        //    { "max-width": "1px", "targets": 0 }
        //],
        "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
         
            if (aData.Color)
                $('td', nRow).css('background-color', aData.Color);
          

        },

     
        "responsive": true,
        "searching": IsDataSearch,
        "lengthChange": true,
        "destroy": true,
        "processing": true,
        "serverSide": true,
        "orderMulti": false,
        "paging": true,
        "scrollX": true,
        //   "responsive": false,

        //"dom": 'rt<"bottom"il>p<"clear">',

        "pageLength": _pageLength,
        lengthMenu: [[10, 20, 30, 100000], [10, 20, 30, 'All']]
    });

    $('.dataTables_filter input').attr('placeholder', SearchBH);
}
var Utils = Utils || {};
(function (namespace) {
    'use strict';

    // Property to hold public variables and functions
    let exposed = namespace;

    /**
    * FUNCTION: getTimespan
    * USE: Returns the timespan between two dates, or a time value in seconds
    * @param start: The starting time
    * @param end: The ending time
    * @return: An object which contains information about the timespan
    */
    exposed.getTimespan = (start, end = null) => {
        // Get the seconds
        let seconds = Number(start);
        if (isDate(start) && isDate(end)) {
            seconds = exposed.getDurationSeconds(start, end);
        }

        // Convert seconds to time
        let timespan = {};
        let remainingTime = seconds;
        for (const def of conversion) {
            // Get time conversion
            timespan[def.key] = Math.floor(remainingTime / def.value);
            remainingTime %= def.value;

            // Add additional info 
            let additionalKey = `total${capitalize(def.key)}`;
            timespan[additionalKey] = seconds / def.value;
        }

        // Add additional info
        timespan.toString = () => {
            let string = '';
            for (const def of conversion) {
                let value = timespan[def.key]
                if (value != 0) {
                    let name = def.key;
                    if (name.endsWith('s')) {
                        name = name.substring(0, name.length - 1);
                    }
                    string += string.length > 0 ? ',' : '';
                    string += ` ${value} ${name}${value == 1 ? '' : 's'}`;
                }
            }
            return string.trim();
        }
        return timespan;
    }

    /**
    * FUNCTION: getDurationSeconds
    * USE: Returns the number of seconds between the two dates
    * @param dateStart: The starting date
    * @param dateEnd: The ending date
    * @return: The number of seconds between the two dates
    */
    exposed.getDurationSeconds = (dateStart, dateEnd) => {
        let timeStart = dateStart ? treatAsUTC(dateStart).getTime() : 0;
        let timeEnd = dateEnd ? treatAsUTC(dateEnd).getTime() : 0;
        return (timeEnd - timeStart) / 1000;
    }

    let treatAsUTC = (date) => {
        let result = new Date(date);
        result.setMinutes(result.getMinutes() - result.getTimezoneOffset());
        return result;
    }

    let isDate = (expression) => {
        return expression && (expression instanceof Date);
    }

    let capitalize = (string) => {
        return string.charAt(0).toUpperCase() + string.substring(1);
    }

    // Set the time conversion definitions (from seconds)                
    const conversion = [
        { key: 'years', value: 60 * 60 * 24 * 365 },
        { key: 'days', value: 60 * 60 * 24 },
        { key: 'hours', value: 60 * 60 },
        { key: 'minutes', value: 60 },
        { key: 'seconds', value: 1 },
        { key: 'milliseconds', value: 1 / 1000 }
    ];

    (function (factory) {
        if (typeof define === 'function' && define.amd) {
            define([], factory);
        } else if (typeof exports === 'object') {
            module.exports = factory();
        }
    }(function () { return namespace; }));
}(Utils)); // http://programmingnotes.org/

