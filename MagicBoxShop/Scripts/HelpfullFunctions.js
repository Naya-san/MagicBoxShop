function onPush() {
    var table = document.getElementById("images");
    var i = table.rows.length - 1;
    var row = table.insertRow(i);
    var cell1 = row.insertCell(0);
    var cell2 = row.insertCell(1);
    cell1.innerHTML = "Изображение №" + (i + 1);
    cell2.innerHTML = "<input type=\"file\" name=\"uploadFile\" accept=\"image/*,image/jpeg\">";
}