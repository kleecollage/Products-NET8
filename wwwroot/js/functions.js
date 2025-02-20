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