
 function GetData()
 {
    var requestOptions = {
        method: 'GET',
        redirect: 'follow'
      };
    fetch("https://localhost:44304/api/main/getAllProduct", 
    requestOptions)
    .then(response => response.text())
    .then(response => {
      var html_card = [];
        var result = JSON.parse(response);
        result.data.forEach(element => {
          var item = `<div class="col-md-3">
          <div class="card">
              <div class="card-header img-fluid">
                  <img src="./images/Group 55441.png" width="100%" height="100%" alt="">
              </div>
              <div class="card-body ">
                  <h5 class="card-title">${element.productName}</h5>
                  <p class="card-text">Some quick example text to build on the card title and make up the bulk of the card's content.</p>
              
              </div>
              <div class="card-footer">
                  <a href="#" class="card-link">${element.productCost} $</a>
                  <a href="./details.html?ProductId=${element.productId}" class="card-link">Details</a>
              </div>
          </div>
        </div>`;
      html_card.push(item);
        });
        var child = document.createElement('div');
        child.className = "row"
        child.innerHTML = html_card.join(" ");
        document.getElementById("root").appendChild(child)
    })
    .catch(error => console.log('error', error));
    
  }
GetData();
function AddUser() {
    console.log("kirdi");
    var myHeaders = new Headers();
    myHeaders.append("Content-Type", "application/json");
    myHeaders.append("charset", "utf-8");
    var raw = JSON.stringify({
        "UserName": document.getElementById("exampleInputEmail").value,
        "Email": document.getElementById("exampleInputUserName").value,
        "Password": document.getElementById("exampleInputPassword").value
    });
    var requestOptions = {
        method: 'POST',
        headers: myHeaders,
        body: raw,
        redirect: 'follow'
    };

    fetch("https://localhost:44304/api/main/signIn", requestOptions)
        .then(response => response.text())
        .then(result => {
            var data = JSON.parse(result);
            console.log(data);
            alert(data.message);
            console.log(data);
        })
        .catch(error => console.log('error', error));

}