export default function SomeList() {
  const array = [
    {label: 'Hello', color: 'red'},
    {label: 'Some', color: 'green'},
    {label: 'Guy', color: 'blue'},
    {label: 'Man', color: 'pink'},
  ];
  return (
    <>
      {array.map((item, i) => (
        <div  key={i}>
          <label style={{color: item.color}}>{item.label}</label><br/>
        </div>
      ))}
    </>
  )
}