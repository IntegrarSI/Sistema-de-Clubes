function formatearJSONDate(fecha) {
    var value = new Date(parseInt(fecha.replace(/(^.*\()|([+-].*$)/g, '')));
    var dat = padDigits(value.getDate(), 2) +
                "/" + padDigits(value.getMonth() + 1, 2) +
                "/" + value.getFullYear();

    return dat;
}

function stringToDate(fecha) {
    return new Date(
        parseInt(fecha.split('-')[0], 10),
        parseInt(fecha.split('-')[1], 10) - 1,
        parseInt(fecha.split('-')[2], 10),
        parseInt(fecha.split('T')[1].split(':')[0], 10),
        parseInt(fecha.split('T')[1].split(':')[1], 10),
        parseInt(fecha.split('T')[1].split(':')[2], 10)
    );
}

function dateToStringYMD(value) {
    var dat = value.getFullYear() +
                "/" + padDigits(value.getMonth() + 1, 2) +
                "/" + padDigits(value.getDate(), 2);

    return dat;
}

function padDigits(number, digits) {
    return Array(Math.max(digits - String(number).length + 1, 0)).join(0) + number;
}

function diferenciaDias(date1, date2) {

    // The number of milliseconds in one day
    var ONE_DAY = 1000 * 60 * 60 * 24

    // Convert both dates to milliseconds
    var date1_ms = date1.getTime()
    var date2_ms = date2.getTime()

    // Calculate the difference in milliseconds
    var difference_ms = Math.abs(date1_ms - date2_ms)

    // Convert back to days and return
    return Math.round(difference_ms / ONE_DAY)

}

function diferenciaHoras(date1, date2) {

    // The number of milliseconds in one day
    var ONE_HOUR = 1000 * 60 * 60;

    // Convert both dates to milliseconds
    var date1_ms = date1.getTime()
    var date2_ms = date2.getTime()

    // Calculate the difference in milliseconds
    var difference_ms = Math.abs(date1_ms - date2_ms)

    // Convert back to days and return
    return Math.round(difference_ms / ONE_HOUR)

}

function diferenciaMinutos(date1, date2) {

    // The number of milliseconds in one day
    var ONE_HOUR = 1000 * 60;

    // Convert both dates to milliseconds
    var date1_ms = date1.getTime()
    var date2_ms = date2.getTime()

    // Calculate the difference in milliseconds
    var difference_ms = Math.abs(date1_ms - date2_ms)

    // Convert back to days and return
    return Math.round(difference_ms / ONE_HOUR)

}

Date.prototype.addDays = function (days) {
    var dat = new Date(this.valueOf());
    dat.setDate(dat.getDate() + days);
    return dat;
}

function nombreMes(mes) {
    var month = new Array();
    month[0] = "Enero";
    month[1] = "Febrero";
    month[2] = "Marzo";
    month[3] = "Abril";
    month[4] = "Mayo";
    month[5] = "Junio";
    month[6] = "Julio";
    month[7] = "Agosto";
    month[8] = "Septiembre";
    month[9] = "Octubre";
    month[10] = "Noviembre";
    month[11] = "Diciembre";
    return month[mes-1];
}

function nombreDia(dia) {
    var dias = new Array();
    dias[0] = "Domingo";
    dias[1] = "Lunes";
    dias[2] = "Martes";
    dias[3] = "Miércoles";
    dias[4] = "Jueves";
    dias[5] = "Viernes";
    dias[6] = "Sábado";
    return dias[dia];
}