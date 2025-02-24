function greetings()
{
  alert("Hello, World!");
}

function confirmSweet(ask, route)
{
  Swal.fire({
    title: ask,
    icon: 'error',
    confirmButtonText: 'Yes',
    confirmButtonColor: '#3085d6',
    // showCancelButton: true,
    showDenyButton: true,
    denyButtonText: 'No',
    denyButtonColor: '#d33',
  }).then((result) => {
    if (result.isConfirmed) {
      window.location = route;
    }
  });
}

function searchEngine()
{
  if (document.getElementById("search").value == "") return false;
  document.form_search.submit();
}

function logout(route)
{
  Swal.fire({
    title: "Are you sure you want to log out?",
    icon: 'info',
    confirmButtonText: 'Logout',
    confirmButtonColor: '#3085d6',
    // showCancelButton: true,
    showDenyButton: true,
    denyButtonText: 'Cancel',
    denyButtonColor: '#d33',
  }).then((result) => {
    if (result.isConfirmed) {
      window.location = route;
    }
  })
}