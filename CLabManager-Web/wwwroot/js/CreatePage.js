var idCount = 0

function getCookie(cname) {
    let name = cname + "=";
    let decodedCookie = decodeURIComponent(document.cookie);
    let ca = decodedCookie.split(';');
    for (let i = 0; i < ca.length; i++) {
        let c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}

$(document).ready(function () {
    if (lab) {
        console.log("this is room no", lab.RoomNo)
        console.log("this is building no", lab.BuildingNo)
        console.log("this is grid type", lab.GridType)

        if (lab.GridType === 1) {
            drawGridType1()
        } else if (lab.GridType === 2) {
            drawGridType2()
        }
        assignComputersToGrid()
    }
    console.log("this is lab" ,lab?.LabId)
    
    $("#computerCreationSave").click(function () {
        let formData = JSON.stringify({
            computerName: $('#computerName').val(),
            description: $('#computerDesc').val()
        })
        console.log(formData)
        $.ajax({
            type: "POST",
            url: "https://localhost:7138/api/Computers",
            headers: {
                'content-type': 'application/json;',
                'Authorization': 'Bearer '+ getCookie('X-Access-Token')
            },
            data: formData,
            success: () => {
                var mydiv = document.querySelector("#unassignedcomps")
                fetch("https://localhost:7138/api/Computers/unassigned", { headers: { 'Authorization': 'Bearer ' + getCookie('X-Access-Token') }})
                    .then(response => response.json())
                    .then(computers => {
                        mydiv.innerHTML = '';
                        computers.map(computer => {
                            console.log(computer)
                            var firstdiv = document.createElement('div')
                            firstdiv.setAttribute('shouldDelete', true)
                            var seconddiv = document.createElement('div')
                            var thirddiv = document.createElement('div')
                            var p = document.createElement('p')
                            var b = document.createElement('b')
                            b.innerHTML = computer.computerName
                            p.appendChild(b)
                            thirddiv.appendChild(p)
                            thirddiv.classList.add("col")
                            firstdiv.classList.add("m-2", "row", "border", "bg-white")
                            seconddiv.classList.add("col")
                            firstdiv.appendChild(seconddiv)
                            firstdiv.appendChild(thirddiv)
                            var myimg = document.createElement('img')
                            myimg.id = computer.computerId
                            myimg.name = computer.computerName
                            console.log("image id", myimg.id)
                            console.log("image name",myimg.name)
                            myimg.src = "/images/ComputerIcon.png"
                            myimg.alt = "computer"
                            myimg.style.width = "80px"
                            myimg.style.height = "80px"
                            myimg.draggable = true
                            myimg.ondragstart = dragStart_handler
                            seconddiv.appendChild(myimg)
                            console.log("this was ran")
                            mydiv.appendChild(firstdiv)
                        })
                    });
                $('#computerName').val('')
                $('#computerDesc').val('') 
            }
        })
        console.log("ajax request")
    })
})

function dragStart_handler(event) {
    console.log("in dragstart")
    console.log(event)
    event.dataTransfer.dropEffect = "move"
    event.dataTransfer.setData("text/plain", event.target.id)
}

function drop_unassigned_handler(event) {
    event.preventDefault()
    const data = event.dataTransfer.getData("text/plain")
    var div = document.getElementById("unassignedcomps")
    var img = document.getElementById(data)
    if (img.parentElement.parentElement.hasAttribute('shouldDelete')) {
        img.parentElement.parentElement.remove()
    }
    var firstdiv = document.createElement('div')
    var seconddiv = document.createElement('div')
    var thirddiv = document.createElement('div')
    var p = document.createElement('p')
    var b = document.createElement('b')
    b.innerHTML = img.name
    console.log("this is img.name" ,img.name)
    p.appendChild(b)
    thirddiv.appendChild(p)
    thirddiv.classList.add("col")
    firstdiv.setAttribute('shouldDelete', true)
    firstdiv.classList.add("m-2", "row","border", "bg-white")
    seconddiv.classList.add("col")
    seconddiv.appendChild(img)
    firstdiv.appendChild(seconddiv)
    firstdiv.appendChild(thirddiv)
    div.appendChild(firstdiv)

    var options = {
        method: "PUT",
        headers: {
            'content-type': 'application/json',
            'Authorization': 'Bearer ' + getCookie('X-Access-Token')
        },
        body: JSON.stringify({
            positionOnGrid: null,
            isPositioned: false,
            labId: null
        })
    }
    fetch(`https://localhost:7138/api/Computers/ispositioned/${img.id}`, options)
        .then(() => console.log("fetch executed for drop"))
    console.log("unassigned drop")
}

function drag_unassigned_handler(event) {
    event.preventDefault()
    event.dataTransfer.dropEffect = "move"
}


function createSingleAllotment() {
    var box = document.createElement('div');
    box.id = idCount++
    box.style.height = '100px';
    box.style.width = '100px';
    box.style.background = 'grey';
    box.style.border = "10px solid black"
    box.style.margin = "5px";
    box.style.padding = "0px"
    box.classList.add("col-2")
    box.ondragover = function dragOver_handler(event) {
        event.preventDefault()
        event.dataTransfer.dropEffect = "move"
    }
    box.ondrop = function drop_handler(event) {
        event.preventDefault()
        
        if (!box.hasChildNodes()) {
            const data = event.dataTransfer.getData("text/plain");
            var img = document.getElementById(data)
            if (img.parentElement.parentElement.hasAttribute('shouldDelete')) {
                img.parentElement.parentElement.remove()
            }
            box.appendChild(img)
            var options = {
                method: "PUT",
                headers: {
                    'content-type': 'application/json',
                    'Authorization': 'Bearer ' + getCookie('X-Access-Token')
                    },
                body: JSON.stringify({
                    positionOnGrid: box.id,
                    isPositioned: true,
                    labId: lab.LabId
                })
            }
            console.log("this is box id",box.id)
            fetch(`https://localhost:7138/api/Computers/ispositioned/${img.id}`,options)
                .then(() => console.log("fetch executed for drop"))
        } 
        console.log(box)
    }
    return box;
}

function assignComputersToGrid() {
    lab.ComputerList.map(computer => {
        var myimg = document.createElement('img')
        myimg.id = computer.ComputerId
        myimg.name = computer.ComputerName
        console.log('sexy id',myimg.id)
        myimg.src = "/images/ComputerIcon.png"
        myimg.alt = "computer"
        myimg.style.width = "80px"
        myimg.style.height = "80px"
        myimg.draggable = true
        myimg.ondragstart = dragStart_handler
        console.log('#' + computer.PositionOnGrid)
        //document.getElementById(computer.PositionOnGrid).appendChild(myimg)
        document.querySelector(`#gridContainer #\\3${computer.PositionOnGrid} `).appendChild(myimg)
    })
}

function drawGridType1() {
    var container = document.getElementById('gridContainer');  

    function createTeacherRow() {
        console.log("teacher row")
        var teachersRow = document.createElement('div')
        teachersRow.classList.add("row")
        teachersRow.appendChild(createSingleAllotment())
        container.appendChild(teachersRow)
    }

    function createstudentRow() {
        console.log("student row")
        var emptyDiv = document.createElement('div')
        emptyDiv.classList.add("col-2")
        emptyDiv.style.height = '100px'
        emptyDiv.style.width = '100px'
        var studentsRow = document.createElement('div')
        studentsRow.classList.add("row")
        studentsRow.appendChild(createSingleAllotment())
        studentsRow.appendChild(createSingleAllotment())
        studentsRow.appendChild(emptyDiv)
        studentsRow.appendChild(createSingleAllotment())
        studentsRow.appendChild(createSingleAllotment())
        container.appendChild(studentsRow)
    }

    createTeacherRow()
    createstudentRow()
    createstudentRow()
    createstudentRow()

}

function drawGridType2() {
    var container = document.getElementById('gridContainer');

    function createTeachersRow() {
        var teachersRow = document.createElement('div')
        teachersRow.classList.add("row")
        teachersRow.classList.add("justify-content-center")
        teachersRow.appendChild(createSingleAllotment())
        container.appendChild(teachersRow)

    }
    function createStudentsRow() {

        var studentsRow = document.createElement('div')
        studentsRow.classList.add("row")
        studentsRow.classList.add("d-flex")
        studentsRow.classList.add("justify-content-between")
        studentsRow.appendChild(createSingleAllotment())
        studentsRow.appendChild(createSingleAllotment())
        container.appendChild(studentsRow)
    }
    createTeachersRow()
    createStudentsRow()
    createStudentsRow()
    createStudentsRow()

}

