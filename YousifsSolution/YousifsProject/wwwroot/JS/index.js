
const selectElement = document.querySelector('#sort');
var mySort = ["Address", true]
const maxFloorElement = document.querySelector("#maxFloor");
const minFloorElement = document.querySelector("#minFloor");
var maxFloor = maxFloorElement.value - 3; // -3 to normalize building height.
var minFloor = minFloorElement.value - 3;

var allRoofs = document.querySelectorAll(".myCheck");
var myRoofs = "-";
allRoofs.forEach(o => {
    myRoofs += " " + o.value;
    o.addEventListener("change", function () {
        if (this.checked) {
            myRoofs += " " + this.value;
            getPartialView(mySort)
        }
        else {
            myRoofs = myRoofs.replace(" " + this.value, "");
            getPartialView(mySort)
        }
    })
});


maxFloorElement.addEventListener("change", (event) => {
    maxFloor = event.target.value - 3;
    getPartialView(mySort)
});
minFloorElement.addEventListener("change", (event) => {
    minFloor = event.target.value - 3;
    getPartialView(mySort)
});


selectElement.addEventListener('change', (event) => {
    let text = event.target.options[event.target.selectedIndex].text;
    console.log(text);
    if (text.slice(-10) === "Descending") {
        mySort = [event.target.value, false]
        getPartialView(mySort);
    }
    else {
        mySort = [event.target.value, true]
        getPartialView(mySort);
    }
});
getPartialView(mySort);


function deleteHouse(id) {
    console.log("Deleting");
    fetch(`/delete/${id}`, { method: 'DELETE' })
        .then(async response => console.log(response))
        .then(() => getPartialView(mySort));
}


async function getPartialView(sort) {

    const superContainer = document.querySelector(".super-container");
    await fetch(`indexpartial/?sort=${sort[0]}&isAscending=${sort[1]}&minFloor=${minFloor}&maxFloor=${maxFloor}&roofs=${myRoofs}`, { method: "GET" }).
        then(result => result.text()).
        then(html => {
            superContainer.innerHTML = html;
        });


    // Code above this is responsible for sorting order//
    // Code below this is responsible for dragging //


    // ------ Makes houses draggable ------------
    const draggables = document.querySelectorAll(".draggable");
    const container = document.querySelector(".container");
    draggables.forEach((draggable) => {
        draggable.addEventListener("dragstart", (e) => {
            console.log("test");
            draggable.classList.add("dragging");
        });

        draggable.addEventListener("dragend", () => {
            draggable.classList.remove("dragging");
            saveMove()
            console.log("1")
        });
        container.addEventListener("dragover", (e) => {
            e.preventDefault();
            const afterElement = getDragAfterElement(container, e.clientX);
            const draggable = document.querySelector(".dragging");

            if (afterElement == null) {
                container.appendChild(draggable);
            } else {
                container.insertBefore(draggable, afterElement);
            }
        });
    });


    function getDragAfterElement(container, x) {
        const draggableElements = [
            ...container.querySelectorAll(".draggable:not(.dragging)"),
        ];
        return draggableElements.reduce(
            (closest, child) => {
                const box = child.getBoundingClientRect();
                const offset = x - box.left - box.width / 2;
                //console.log(x);
                //console.log(offset);
                if (offset < 0 && offset > closest.offset) {
                    return { offset: offset, element: child };
                } else {
                    return closest;
                }
            },
            {
                offset: Number.NEGATIVE_INFINITY,
            }
        ).element;
    }
}

function getMenu(id) {
    let x = document.getElementById(id);
    if (x.dataset.height == 0) {
        x.dataset.height = 10;
        x.style.height = "10rem";
    } else {
        x.dataset.height = 0;
        x.style.height = "0";
    }
}

function saveMove() {
    let ids = [...document.querySelectorAll(".house")].map((o) => o.dataset.id);
    query = "/saveMovings/?";
    for (var i = 0; i < ids.length; i++) {
        query += `idArray=${ids[i]}`
        if (i != ids.length - 1) {
            query += "&"
        }
    }
    console.log(query);
    fetch(query, { method: "POST" })
}

function filterMouseOut() {
    let filter = document.querySelector(".filter");
    filter.style.height = 0;
    filter.style.padding = 0;
}

function filterMouseOver() {
    let filter = document.querySelector(".filter");
    filter.style.height = "15rem";
    filter.style.padding = "1rem";
}





