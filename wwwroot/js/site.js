const uri = 'api/todoitems';
let todos = [];

function getItems() {
    fetch(uri)
        .then(response => response.json())
        .then(data => _displayItems(data))
        .catch(error => console.error('Unable to get items.', error));
}

function getByName() {
    const inputElement = document.getElementById('filter-name');
    const inputValue = inputElement.value;

    fetch('api/todoitems/filter?name=' + inputValue)
        .then(response => response.json())
        .then(data => _displayItems(data))
        .catch(error => console.error('Unable to filter items.', error));
}


function addItem() {
    const addNameTextbox = document.getElementById('add-name');

    const item = {
        isComplete: false,
        name: addNameTextbox.value.trim(),
        surname: document.getElementById('add-surname').value.trim(),
        email: document.getElementById('add-email').value.trim(),
        phone: document.getElementById('add-phone').value.trim()
    };

    fetch(uri, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(item)
    })
        .then((response) => {
            console.log("Reponse status is " + response.status);
            if (response.status == 201) {
                addNameTextbox.value = '';
                document.getElementById('add-surname').value = '';
                document.getElementById('add-email').value = '';
                document.getElementById('add-phone').value = '';
                document.getElementById('add-error-label').innerText = "";
            } else {
                document.getElementById('add-error-label').innerText = "Bad request: " + response.statusText;
            }
            return response.json();
        })
        .then(() => {
            getItems();
        })
        .catch(error => console.error('Unable to add item.', error));
}

function deleteItem(id) {
    fetch(`${uri}/${id}`, {
        method: 'DELETE'
    })
        .then(() => getItems())
        .catch(error => console.error('Unable to delete item.', error));
}

function displayEditForm(id) {
    const item = todos.find(item => item.id === id);

    document.getElementById('edit-id').value = item.id;
    document.getElementById('edit-name').value = item.name;
    document.getElementById('edit-surname').value = item.surname;
    document.getElementById('edit-email').value = item.email;
    document.getElementById('edit-phone').value = item.phone;
    document.getElementById('editForm').style.display = 'block';

    clickTop();
}

function updateItem() {
    const itemId = document.getElementById('edit-id').value;
    const item = {
        id: parseInt(itemId, 10),
        name: document.getElementById('edit-name').value.trim(),
        surname: document.getElementById('edit-surname').value.trim(),
        email: document.getElementById('edit-email').value.trim(),
        phone: document.getElementById('edit-phone').value.trim()
    };

    fetch(`${uri}/${itemId}`, {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(item)
    })
        .then(() => getItems())
        .catch(error => console.error('Unable to update item.', error));

    closeInput();

    return false;
}

function closeInput() {
    document.getElementById('editForm').style.display = 'none';
}

function closeMessage() {
    document.getElementById('messageContent').style.display = 'none';
}

function _displayCount(itemCount) {
    const name = (itemCount === 1) ? 'contact' : 'contacts';

    document.getElementById('counter').innerText = `${itemCount} ${name} displayed`;
}

function _displayItems(data) {
    const tBody = document.getElementById('todos');
    tBody.innerHTML = '';

    _displayCount(data.length);

    const button = document.createElement('button');

    data.forEach(item => {
        let isCompleteCheckbox = document.createElement('input');
        isCompleteCheckbox.type = 'checkbox';
        isCompleteCheckbox.disabled = true;
        isCompleteCheckbox.checked = item.isComplete;

        let editButton = button.cloneNode(false);
        editButton.innerText = 'Edit';
        editButton.setAttribute('onclick', `displayEditForm(${item.id})`);

        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = 'Delete';
        deleteButton.setAttribute('onclick', `deleteItem(${item.id})`);

        let tr = tBody.insertRow();

        let td1 = tr.insertCell(0);
        let textNodeId = document.createTextNode(item.id);
        td1.appendChild(textNodeId);

        let td2 = tr.insertCell(1);
        let textNode = document.createTextNode(item.name);
        td2.appendChild(textNode);
        
        let td3 = tr.insertCell(2);
        let textNode2 = document.createTextNode(item.surname);
        td3.appendChild(textNode2);

        let td4 = tr.insertCell(3);
        let textNode3 = document.createTextNode(item.email);
        td4.appendChild(textNode3);

        let td5 = tr.insertCell(4);
        let textNode4 = document.createTextNode(item.phone);
        td5.appendChild(textNode4);
        
        let td6 = tr.insertCell(5);
        td6.appendChild(editButton);

        let td7 = tr.insertCell(6);
        td7.appendChild(deleteButton);
    });

    todos = data;
}

function clickTop() {
    var anchor = document.getElementById("anchor-top");
    if (anchor) {
        anchor.click();
    }
}

function clickSurname() {
    fetch(uri + "/order")
        .then(response => response.json())
        .then(data => _displayItems(data))
        .catch(error => console.error('Unable to get items.', error));
}
