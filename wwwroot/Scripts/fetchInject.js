fetch('cookies')
    .then(response => response.json())
    .then(cookies => {
        var table = document.getElementById('test1');
        //console.log(cookies);

        var rootDiv = document.createElement('div');

        var header = document.createElement('h2');
        header.textContent = "Cookies from injected javascript:";

        rootDiv.appendChild(header);

        Object.keys(cookies).forEach(key => {
            var inside = document.createElement('div');
            inside.style = "padding: 10px;";
            inside.textContent = cookies[key].key + ' => ' + cookies[key].value;

            rootDiv.appendChild(inside);
        });
        
        table.parentNode.insertBefore(rootDiv, table.nextSibling);
    });